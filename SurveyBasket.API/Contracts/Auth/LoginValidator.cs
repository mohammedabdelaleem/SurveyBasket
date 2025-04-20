using FluentValidation;

namespace SurveyBasket.API.Contracts.Auth;

public class LoginValidator : AbstractValidator<LoginRequest>
{
	public LoginValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();


		RuleFor(x => x.Password)
			.NotEmpty();


	}
}
