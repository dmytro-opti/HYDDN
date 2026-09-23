using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Repositories;

/// <summary>
/// Generic EF Core CRUD repository. Every write operation is saved immediately.
/// </summary>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
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
        await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        // tracked entities are picked up by change tracking, detached ones have to be attached
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Update(entity);
        }

        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        DbSet.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
