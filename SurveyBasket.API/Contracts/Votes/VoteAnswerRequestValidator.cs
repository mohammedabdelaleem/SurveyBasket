namespace SurveyBasket.API.Contracts.Votes;

public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
{

	// VoteAnswerRequest is a child from VoteRequest 
	// you need to append validation for children at parent

	public VoteAnswerRequestValidator()
	{
		RuleFor(x => x.QuestionId)
			.GreaterThanOrEqualTo(1);

		RuleFor(x => x.AnswerId)
			.GreaterThanOrEqualTo(1);
	}
}
