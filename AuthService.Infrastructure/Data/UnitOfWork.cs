using System.Reflection.Metadata;
using AuthService.Application.Common.Interfaces.Persistence;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    
    #region Properties
    
    private Repository<User, int> _userRepository;
    public IRepository<User, int> UserRepository =>
        _userRepository ?? (_userRepository = new Repository<User, int>(_dbContext));
    
    #endregion
    
    private readonly ApplicationDbContext _dbContext;
    private bool _disposed = false;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }
 
    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)         
            {
                await _dbContext.DisposeAsync();
            }
            _disposed = true;
        }
    }
}