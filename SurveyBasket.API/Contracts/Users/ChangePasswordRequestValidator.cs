using SurveyBasket.API.Abstractions.Consts;

namespace SurveyBasket.API.Contracts.Users;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
	public ChangePasswordRequestValidator()
	{

		RuleFor(x => x.CurrentPassword)
			.NotEmpty();

		RuleFor(x => x.NewPassword)
			.NotEmpty()
			.Matches(RegexPatterns.Password)
			.WithMessage("Password Should Be At Least 8 Digits And Should Contains Lowercase , Uppercase And NonAlphanumeric")
			.NotEqual(x => x.CurrentPassword)
			.WithMessage("New Password Can't Be The Same Old Password");


			}
}