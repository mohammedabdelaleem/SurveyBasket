using SurveyBasket.API.Contracts.Results;

namespace SurveyBasket.API.Services;

public interface IResultService
{
	Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default);
	Task<Result<IEnumerable<VotesPerDayRespnse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default);
}
