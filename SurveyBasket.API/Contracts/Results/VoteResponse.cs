namespace SurveyBasket.API.Contracts.Results;

public record VoteResponse(
	string VoterName,
	DateTime VotingDate,
	 IEnumerable<QuestionAnswerResponse> SelectedAnswers
	);
