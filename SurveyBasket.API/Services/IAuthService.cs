

namespace SurveyBasket.API.Services;

public interface IAuthService
{
	// Takes Username and password 
	// Check Credintiality
	// return set of values || Null If The User Credintial Is Null 

	// Service For getting username and pass <==> JWT Procider Generate Token And Send Back
	Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
	Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

	Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
	Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
	Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);

	Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequest request);
	Task<Result> SendResetPasswordCodeAsync(string email);

}