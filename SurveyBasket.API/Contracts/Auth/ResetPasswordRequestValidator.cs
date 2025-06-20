using SurveyBasket.API.Abstractions.Consts;

namespace SurveyBasket.API.Contracts.Auth;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
	public ResetPasswordRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();

		RuleFor(x => x.Code)
		.NotEmpty();

		RuleFor(x => x.NewPassword)
			.NotEmpty()
			.Matches(RegexPatterns.Password)
			.WithMessage("Password Should Be At Least 8 Digits And Should Contains Lowercase , Uppercase And NonAlphanumeric");
	}
}
