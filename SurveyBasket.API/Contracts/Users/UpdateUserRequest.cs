namespace SurveyBasket.API.Contracts.Users;

public record UpdateUserRequest
(
	string FirstName,
	string LastName,
	string Email,
	IList<string> Roles
	);
