using SurveyBasket.API.Contracts.Votes;

namespace SurveyBasket.API.Services;

public interface IVoteService
{
	Task<Result> AddVoteAsync(int pollId, string userId, VoteRequest voteRequest, CancellationToken cancellationToken = default);
}
