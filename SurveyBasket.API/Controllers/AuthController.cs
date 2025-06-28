
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.API.Abstractions.Consts;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace SurveyBasket.API.Controllers;
[Route("[controller]")]
[ApiController]
[EnableRateLimiting(RateLimiterInfo.IpAddressPolicy)]
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]

public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
	private readonly IAuthService _authService = authService;
	private readonly ILogger<AuthController> _logger = logger;

		
	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[DisableRateLimiting]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
	{
		var result = await _authService.RegisterAsync(request, cancellationToken);

		return (result.IsSuccess) ? Ok() : result.ToProblem();
	}


	[HttpPost("confirm-email")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
	{
		var result = await _authService.ConfirmEmailAsync(request);
		return (result.IsSuccess) ? Ok() : result.ToProblem();
	}

	[HttpPost("resend-confirmation-email")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationRequest request)
	{
		var result = await _authService.ResendEmailConfirmationAsync(request);
		return (result.IsSuccess) ? Ok() : result.ToProblem();
	}


	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Logging With Email: {email} , Password :{password}", request.Email, request.Password); //use variables here not string Interpolation $ ==> It's Better Searchig for variable[key] called email with value like test@test.com than searchig for a word called email 
		var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

		return (authResult.IsSuccess) ? Ok(authResult.Value) : authResult.ToProblem();
	}

	[HttpPost("refresh")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
	{
		var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
		return (authResult.IsSuccess) ? Ok(authResult.Value) : authResult.ToProblem();
	}


	[HttpPost("revoke-refresh-token")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
	{
		var authResult = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
		return (authResult.IsSuccess) ? Ok() : authResult.ToProblem();
	}


	[HttpPost("forget-password")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]

	public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequset request)
	{
		var authResult = await _authService.SendResetPasswordCodeAsync(request.Email);
		return (authResult.IsSuccess) ? Ok() : authResult.ToProblem();
	}



	[HttpPost("reset-password")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]

	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
	{
		var authResult = await _authService.ResetPasswordAsync(request);
		return (authResult.IsSuccess) ? Ok() : authResult.ToProblem();
	}



}
