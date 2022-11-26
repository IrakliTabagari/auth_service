namespace AuthService.Application.Services.Login;

public interface ILoginService
{
    LoginResult Login(string email, string password);
}