namespace AuthService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public AuthController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginWithEmailAndPasswordRequest request)
    {
        var loginResult = await _loginService.Login(request.Email, request.Password);
        
        
        return Ok(response);
    }
}