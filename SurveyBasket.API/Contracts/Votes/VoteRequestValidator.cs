namespace SurveyBasket.API.Contracts.Votes;

public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
	public VoteRequestValidator()
	{
		RuleFor(x => x.Answers)
			.NotEmpty();

		// you need to append validation for children at parent
		RuleForEach(x => x.Answers)
			.SetInheritanceValidator(x => x.Add(new VoteAnswerRequestValidator()));
	}
}
