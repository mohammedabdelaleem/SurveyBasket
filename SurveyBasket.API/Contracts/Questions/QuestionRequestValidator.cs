namespace SurveyBasket.API.Contracts.Questions;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
	public QuestionRequestValidator()
	{
		RuleFor(x => x.Content)
			.NotEmpty()
			.Length(3, 1000);


		// at least 2 answers
		RuleFor(x => x.Answers)
			.Must(x=>x.Count >= 2)
			.WithMessage("Answers Must Be At Least 2");

		//distinct answers
		RuleFor(x => x.Answers)
			.Must(x => x.Count == x.Distinct().Count())
			.WithMessage("Only Distinct Answers");
	}
}
