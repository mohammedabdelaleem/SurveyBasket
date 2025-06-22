namespace SurveyBasket.API.Authentication;

public interface IJWTProvider
{
	(string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);



	// Check incomming jwt token ==> if ok return user id from its claims
	// ? because may be the incomming jwt is not valid


	//return user id
	string? ValidateTokenAndGetUserId(string token);
}
