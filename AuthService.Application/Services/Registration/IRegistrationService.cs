namespace AuthService.Application.Services.Registration;

public interface IRegistrationService
{
    RegistrationResult Register(string firstName, string lastName, string email, string password);
}