
namespace SurveyBasket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
	private readonly IAuthService _authService = authService;
	private readonly ILogger<AuthController> _logger = logger;

	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult<AuthResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Logging With Email: {email} , Password :{password}", request.Email, request.Password); //use variables here not string Interpolation $ ==> It's Better Searchig for variable[key] called email with value like test@test.com than searchig for a word called email 
		var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

		return (authResult.IsSuccess) ? Ok(authResult.Value) : authResult.ToProblem();
	}

	[HttpPost("refresh")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult<AuthResponse>> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
	{
		var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
		return (authResult.IsSuccess) ? Ok(authResult.Value) : authResult.ToProblem();
	}


	[HttpPost("revoke-refresh-token")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult<AuthResponse>> RevokeAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
	{
		var authResult = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
		return (authResult.IsSuccess) ? Ok() : authResult.ToProblem();
	}
}
