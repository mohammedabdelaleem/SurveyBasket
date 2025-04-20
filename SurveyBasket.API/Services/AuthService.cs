
using SurveyBasket.API.Authentication;

namespace SurveyBasket.API.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJWTProvider jWTProvider) : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager = userManager;
	private readonly IJWTProvider _jWTProvider = jWTProvider;

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

		// Generate JWT Token 
		var (token, expiresIn) = _jWTProvider.GenerateToken(user);

		// Return Response
		return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);
	}
}
