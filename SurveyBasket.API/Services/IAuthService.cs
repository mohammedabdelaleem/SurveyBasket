
namespace SurveyBasket.API.Services;

public interface IAuthService
{
	// Takes Username and password 
	// Check Credintiality
	// return set of values || Null If The User Credintial Is Null 

	// Service For getting username and pass <==> JWT Procider Generate Token And Send Back

	Task<AuthResponse?> GetTokenAsync(string email , string password, CancellationToken cancellationToken=default);
}
