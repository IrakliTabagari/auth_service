using AuthService.Application.Common.Interfaces.Login;
using AuthService.Application.Common.Interfaces.Persistence;
using AuthService.Domain.Entities;

namespace AuthService.Application.Services.Registration;

public class RegistrationService : IRegistrationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrationService(
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegistrationResult> Register(string firstName, string lastName, string email, string password)
    {
        if (_unitOfWork.UserRepository.Any(filter: x => x.Email == email))
        {
            throw new Exception("User with given email already exists");
        }

        var newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            IsActive = true
        };
        
        _unitOfWork.UserRepository.Insert(newUser);
        
        var token = _jwtTokenGenerator.GenerateToken(
            userId: newUser.Id, 
            firstName: newUser.FirstName, 
            lastName: newUser.LastName, 
            email: newUser.Email);

        return new RegistrationResult(
            newUser.Id,
            newUser.FirstName,
            newUser.LastName,
            newUser.Email,
            token
            );
    }
}