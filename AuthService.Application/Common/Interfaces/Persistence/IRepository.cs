using System.Linq.Expressions;
using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces.Persistence;

public interface IRepository<TEntity, TIdentity> where TEntity: BaseEntity<TIdentity>
{
    Task<List<TEntity>> FindListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        string includeProperties = "");
    IQueryable<TEntity> Query();
    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
    Task<TEntity?> GetByIdAsync(TIdentity id);
    Task InsertAsync(TEntity entity);
    Task DeleteAsync(TIdentity id);
    Task DeleteAsync(TEntity entityToDelete);
    Task UpdateAsync(TEntity entityToUpdate);
    Task DeleteRangeAsync(Expression<Func<TEntity, bool>> filter);
}
}