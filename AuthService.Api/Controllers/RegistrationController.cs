namespace AuthService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController
{

    private readonly IRegistrationService _registrationService;

    public AuthController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }


    [HttpPost]
    public async Task<IActionResult> RegisterWithEmail([FromBody] RegisterWithEmailRequest request)
    {
        var response = await _registrationService.Register(request.FirstName, request.LastName, request.Email, request.Password);
        
        return Ok(response);
    }
}