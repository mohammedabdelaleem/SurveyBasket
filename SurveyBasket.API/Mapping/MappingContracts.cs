using SurveyBasket.API.Contracts.Requests;
using SurveyBasket.API.Contracts.Responses;

namespace SurveyBasket.API.Mapping;

public static class MappingContracts
{
	// request {title, id}  ===> Mapping object to Poll for storing Poll Object At DB As Example
	// ex: adding into DB
	public static Poll MappingToPoll(this PollRequest pollRequest)
	{
		return new()
		{
			Title = pollRequest.Title,
			Description = pollRequest.Description,
		};
	}


	// response by only fields you want not the whole object
	public static PollResponse MapToPollResponse(this Poll poll)
	{
		return new()
		{
			Id = poll.Id,
			Title = poll.Title,
			Description = poll.Description,
		};
	}

	public static IEnumerable<PollResponse> MapToPollsResponse(this IEnumerable<Poll> polls)
	{
		//return polls.Select(poll => MapToPollResponse(poll));
		return polls.Select(MapToPollResponse);

	}

}
