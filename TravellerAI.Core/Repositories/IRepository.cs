using System.Linq.Expressions;
using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

/// <summary>
/// Generic CRUD repository for any entity inherited from <see cref="BaseEntity"/>.
/// </summary>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
