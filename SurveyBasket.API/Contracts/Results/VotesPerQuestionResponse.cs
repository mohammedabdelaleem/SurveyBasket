namespace SurveyBasket.API.Contracts.Results;

public record VotesPerQuestionResponse(
	string Title,
	IEnumerable<VotesPerAnswerResponse> SelectedAnswers
	
	);
