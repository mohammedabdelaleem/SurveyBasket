
using SurveyBasket.API.Authentication;
using System.Security.Cryptography;

namespace SurveyBasket.API.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJWTProvider jWTProvider) : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager = userManager;
	private readonly IJWTProvider _jWTProvider = jWTProvider;
	private readonly int _refreshTokenExpirayDays = 14;

	public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
	{
		// check user by email 
		var user = await _userManager.FindByEmailAsync(email);

		if (user == null)
		{
			return null;
		}

		// check password
		bool hasPassword = await _userManager.CheckPasswordAsync(user, password);
		if (!hasPassword)
		{
			return null;
		}

		return await HandleAuthResponse(user);
	}


	public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
	{

		var userId = _jWTProvider.ValidateToken(token);
		if (userId == null)
			return null;


		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return null;


		var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
		if (userRefreshToken == null)
			return null;

		// reaching the point means ==> every thing is ok

		userRefreshToken.RevokedOn = DateTime.UtcNow; // use RefreshToken once : buissness requirement

		return await HandleAuthResponse(user);

	}


	public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
	{
		var userId = _jWTProvider.ValidateToken(token);
		if (userId == null)
			return false;


		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return false;


		var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
		if (userRefreshToken == null)
			return false;

		// reaching the point means ==> every thing is ok

		userRefreshToken.RevokedOn = DateTime.UtcNow; // use RefreshToken once : buissness requirement
		await _userManager.UpdateAsync(user);

		return true;
	}

	private async Task<AuthResponse> HandleAuthResponse(ApplicationUser user)
	{
		// Generate JWT Token 
		var (newToken, expiresIn) = _jWTProvider.GenerateToken(user);

		// generate refresh token
		var newRefreshToken = GenerateRefreshToken();
		var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirayDays);


		// we need to save this new refresh token at the DB 
		// we have the user and the refresh tokens are owned for user

		user.RefreshTokens.Add(new RefreshToken
		{
			Token = newRefreshToken,
			ExpiresOn = refreshTokenExpiration
		});

		await _userManager.UpdateAsync(user);

		// Return Response
		return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName,
			newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
	}
	private string GenerateRefreshToken()
	{
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}

}
