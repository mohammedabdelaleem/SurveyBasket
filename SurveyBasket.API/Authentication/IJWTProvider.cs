namespace SurveyBasket.API.Authentication;

public interface IJWTProvider
{
	(string token, int expiresIn) GenerateToken(ApplicationUser user);
}
