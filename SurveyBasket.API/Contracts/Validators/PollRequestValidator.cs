using FluentValidation;

namespace SurveyBasket.API.Contracts.Validators;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
	public PollRequestValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.Length(3, 100);


		RuleFor(x => x.Summary)
			.NotEmpty()
			.Length(3, 1500);

		RuleFor(x=>x.StartsAt)
			.NotEmpty()
			.GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

		RuleFor(x => x.EndsAt)
			.NotEmpty();

		RuleFor(x => x)
			.Must(HasValidDates)
			.WithName(nameof(PollRequest.EndsAt))
			.WithMessage("{PropertyName} Must Greater Than or Equals Start Date");	

	}

	private bool HasValidDates(PollRequest pollRequest)
	{
		return pollRequest.StartsAt <= pollRequest.EndsAt;
	}
}
