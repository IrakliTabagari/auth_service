using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IRepository<User, int> UserRepository { get; }
    
    void Save();
    void Dispose();
}