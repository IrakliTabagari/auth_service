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

    public LoginResult Login(string email, string password)
    {
        var existeduser = _unitOfWork.UserRepository
            .Query()
            .FirstOrDefault(x => x.Email == email && x.Password == password);
        
        if (existeduser is null)
        {
            throw new Exception("User with given email and password does not exists");
        }

        var token = _jwtTokenGenerator.GenerateToken(
            userId: existeduser.Id, 
            firstName: existeduser.FirstName, 
            lastName: existeduser.LastName,
            email: existeduser.Email);

        return new LoginResult(
            existeduser.Id,
            existeduser.FirstName,
            existeduser.LastName,
            existeduser.Email,
            token
        );
    }
}