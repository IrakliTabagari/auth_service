namespace AuthService.Application.Common.Interfaces.Login;

public interface IJwtTokenGenerator
{
    Task<string> GenerateToken(int userId, string firstName, string lastName, string email);
}