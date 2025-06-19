namespace SurveyBasket.API.Contracts.Users;

public record UserProfileResponse(
	string Email,
	string UserName,
	string FirstName,
	string LastName
	);