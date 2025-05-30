namespace SurveyBasket.API.Authentication;

public interface IJWTProvider
{
	(string token, int expiresIn) GenerateToken(ApplicationUser user);



	// Check incomming jwt token ==> if ok return user id from its claims
	// ? because may be the incomming jwt is not valid

	string? ValidateToken(string token);
}
