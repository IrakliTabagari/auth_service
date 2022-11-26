using System.Linq.Expressions;
using AuthService.Application.Common.Interfaces.Persistence;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data;

public sealed class Repository<TEntity, TIdentity> : IRepository<TEntity, TIdentity> where TEntity : BaseEntity<TIdentity>
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext dbContext)
    {
        this._dbContext = dbContext;
        this._dbSet = dbContext.Set<TEntity>();
    }

    public IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }
    
    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
    {
        return _dbSet.Where(filter).AsQueryable();
    }
    
    public bool Any(Expression<Func<TEntity, bool>> filter)
    {
        return _dbSet.Any(filter);
    }

    public TEntity GetById(TIdentity id)
    {
        return _dbSet.Find(id);
    }

    public void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(TIdentity id)
    {
        var entityToDelete = _dbSet.Find(id);
        Delete(entityToDelete);
    }

    public void Delete(TEntity entityToDelete)
    {
        if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }

    public void Update(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public void DeleteRange(Expression<Func<TEntity, bool>> filter)
    {
        throw new NotImplementedException();
    }
}