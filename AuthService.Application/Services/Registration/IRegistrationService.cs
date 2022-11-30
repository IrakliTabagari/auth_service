namespace AuthService.Application.Services.Registration;

public interface IRegistrationService
{
    Task<RegistrationResult> Register(string firstName, string lastName, string email, string password);
}