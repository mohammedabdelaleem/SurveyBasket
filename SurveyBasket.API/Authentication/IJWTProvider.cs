namespace SurveyBasket.API.Authentication;

public interface IJWTProvider
{
	(string token, int expiresIn) GenerateToken(ApplicationUser user);

	// ? because may be the incomming jwt is not valid
	string? ValidateToken(string token);
}
