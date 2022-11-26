namespace AuthService.Application.Services.Registration;

public record RegistrationResult(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string token
    );