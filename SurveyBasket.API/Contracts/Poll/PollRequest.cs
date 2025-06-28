namespace SurveyBasket.API.Contracts.Poll;

public record PollRequest(
	string Title,
	string Summary,
	DateOnly StartsAt,
	DateOnly EndsAt
	);