namespace AuthService.Application.Services.Login;

public record LoginResult(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string token
);