using SurveyBasket.API.Contracts.Abswers;

namespace SurveyBasket.API.Contracts.Questions;

public record QuestionResponse
	(
		int Id,
		string Content,
		IEnumerable<AnswerResponse> Answers
	);
