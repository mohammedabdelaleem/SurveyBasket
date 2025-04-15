using FluentValidation;

namespace SurveyBasket.API.Contracts.Validators;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
	public PollRequestValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.WithMessage("{PropertyName}  Is Required");

		RuleFor(x => x.Description)
			.NotEmpty()
			.WithMessage("{PropertyName}  Is Required")
			.Length(3,10)
			.WithMessage("{PropertyName} Should Be At Least {MinLength} characters and Maximum {MaxLength} characters you Enterd {TotalLength}");

	}
}
