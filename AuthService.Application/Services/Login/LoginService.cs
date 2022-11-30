using AuthService.Application.Common.Interfaces.Login;
using AuthService.Application.Common.Interfaces.Persistence;

namespace AuthService.Application.Services.Login;

public class LoginService : ILoginService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public LoginService(IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResult> Login(string email, string password)
    {
        var existedUser = await _unitOfWork.UserRepository
            .FindOneAsync(filter: x => x.Email == email && x.Password == password);
        
        if (existedUser is null)
        {
            throw new Exception("User with given email and password does not exists");
        }

        var token = await _jwtTokenGenerator.GenerateToken(
            userId: existedUser.Id, 
            firstName: existedUser.FirstName, 
            lastName: existedUser.LastName,
            email: existedUser.Email);

        return new LoginResult(
            existedUser.Id,
            existedUser.FirstName,
            existedUser.LastName,
            existedUser.Email,
            token
        );
    }
}