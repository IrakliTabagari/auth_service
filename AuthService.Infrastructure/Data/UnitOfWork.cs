using System.Reflection.Metadata;
using AuthService.Application.Common.Interfaces.Persistence;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthDbContext _context;
    
    private Repository<User, int> _userRepository;
    
    public UnitOfWork(AuthDbContext context)
    {
        _context = context;
    }

    public IRepository<User, int> UserRepository
    {
        get
        {
            _userRepository ??= new Repository<User, int>(_context);
            return _userRepository;
        }
    }


    public void Save()
    {
        _context.SaveChanges();
    }

    private bool _disposed = false;



    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}