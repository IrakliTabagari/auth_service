namespace AuthService.Application.Services.Login;

public interface ILoginService
{
    Task<LoginResult> Login(string email, string password);
}