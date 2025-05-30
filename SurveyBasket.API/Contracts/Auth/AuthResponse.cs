namespace SurveyBasket.API.Contracts.Auth;

public record AuthResponse(

	string Id,
	string? Email ,
	string FirstName,
	string LastName,
	string Token,
	int ExpiresIn,
	string RefreshToken,
	DateTime RefreshTokenExpiration // DateTime and Not int because the period may be days or months or years
	// we can also not return Refresh token and its expiration inside auth response , we can set it at the cookies 
	);