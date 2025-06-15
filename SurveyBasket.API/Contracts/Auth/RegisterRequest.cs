namespace SurveyBasket.API.Contracts.Auth;

public record RegisterRequest(
	string Email, // email is the username in my app , if not append the username
	string Password, // we can recive the confirmed email also , but we will let this to the frontend 
	string FirstName,
	string LastName

	);
