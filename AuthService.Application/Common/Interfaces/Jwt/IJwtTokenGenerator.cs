namespace AuthService.Application.Common.Interfaces.Login;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string firstName, string lastName, string email);
}