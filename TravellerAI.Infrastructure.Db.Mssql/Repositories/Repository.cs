using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Repositories;

/// <summary>
/// Generic EF Core CRUD repository. Every write operation is saved immediately.
/// </summary>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    // SQL Server error numbers
    private const int UniqueConstraintViolation = 2627;
    private const int UniqueIndexViolation = 2601;
    private const int ForeignKeyViolation = 547;

    protected readonly TravellerDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(TravellerDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        // tracked entities are picked up by change tracking, detached ones have to be attached
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Update(entity);
        }

        await SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        DbSet.Remove(entity);
        await SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Saves changes and translates provider exceptions into application exceptions.
    /// </summary>
    protected async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entityName = typeof(TEntity).Name.Replace("Entity", string.Empty);

        try
        {
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConflictException($"{entityName} was changed or removed by another request, please retry", ex);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql)
        {
            throw sql.Number switch
            {
                UniqueConstraintViolation or UniqueIndexViolation =>
                    new ConflictException($"{entityName} with the same unique values already exists", ex),
                ForeignKeyViolation =>
                    new ConflictException($"{entityName} cannot be saved or removed because of related data", ex),
                _ => new RepositoryException($"Failed to save {entityName}", ex)
            };
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException($"Failed to save {entityName}", ex);
        }
    }
}
