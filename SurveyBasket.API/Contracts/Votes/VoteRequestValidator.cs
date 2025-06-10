namespace SurveyBasket.API.Contracts.Votes;

public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
	public VoteRequestValidator()
	{
		RuleFor(x => x.Answers)
			.NotEmpty();


		RuleForEach(x=>x.Answers)
			.SetInheritanceValidator(x=>x.Add(new VoteAnswerRequestValidator()));
	}
}
