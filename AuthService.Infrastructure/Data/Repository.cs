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

    public async Task<List<TEntity>> FindListAsync(
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
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>>? filter = null,
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

        return await query.FirstOrDefaultAsync();
    }

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }
    
    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
    {
        return _dbSet.Where(filter).AsQueryable();
    }
    
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
    }

    public async Task<TEntity?> GetByIdAsync(TIdentity id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task DeleteAsync(TIdentity id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        await DeleteAsync(entityToDelete);
    }

    public async Task DeleteAsync(TEntity entityToDelete)
    {
        if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }

    public async Task UpdateAsync(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public async Task DeleteRangeAsync(Expression<Func<TEntity, bool>> filter)
    {
        throw new NotImplementedException();
    }
}