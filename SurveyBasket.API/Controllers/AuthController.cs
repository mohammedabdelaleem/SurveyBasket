﻿
namespace SurveyBasket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
	private readonly IAuthService _authService = authService;

	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult<AuthResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
	{
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
