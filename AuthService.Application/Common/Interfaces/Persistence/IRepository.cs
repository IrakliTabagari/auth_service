using System.Linq.Expressions;
using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces.Persistence;

public interface IRepository<TEntity, TIdentity> where TEntity: BaseEntity<TIdentity>
{
    IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    IQueryable<TEntity> Query();
    
    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter);
    
    bool Any(Expression<Func<TEntity, bool>> filter);

    TEntity GetById(TIdentity id);

    void Insert(TEntity entity);

    void Delete(TIdentity id);

    void Delete(TEntity entityToDelete);

    void Update(TEntity entityToUpdate);

    void DeleteRange(Expression<Func<TEntity, bool>> filter);
}