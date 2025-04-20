namespace SurveyBasket.API.Contracts.Poll;

public record PollRequest(
	string Title ,
	string Summary,
	bool IsPublished,
	DateOnly StartsAt,
	DateOnly EndsAt
	);