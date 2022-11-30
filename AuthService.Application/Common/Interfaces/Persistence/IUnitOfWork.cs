using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<User, int> UserRepository { get; }
    
    Task SaveAsync();
    ValueTask DisposeAsync();
}