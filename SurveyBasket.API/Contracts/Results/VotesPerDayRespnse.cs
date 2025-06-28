namespace SurveyBasket.API.Contracts.Results;

public record VotesPerDayRespnse(
	DateOnly Date,
	int NumberOfVotes
	);
