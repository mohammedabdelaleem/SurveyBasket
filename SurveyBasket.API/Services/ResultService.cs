using SurveyBasket.API.Contracts.Results;
using System.Linq;

namespace SurveyBasket.API.Services;

public class ResultService(AppDbContext context) : IResultService
{
	private readonly AppDbContext _context = context;

	public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken=default)
	{
		var pollVotes = await _context.Polls
			.Where(p=>p.Id == pollId)
			.Select(p => new PollVotesResponse
			(
			 p.Title,
			 p.Votes.Select(v => new VoteResponse
				(
					$"{v.User.FirstName} {v.User.LastName}",
					v.SubmittedOn,
					v.VoteAnswers.Select(a => new QuestionAnswerResponse
					(
						 a.Question.Content,
						 a.Answer.Content
					))
				))
			)).SingleOrDefaultAsync(cancellationToken);


		return pollVotes is null ?
			Result.Failure<PollVotesResponse>(PollErrors.PollNotFound) :
			Result.Success(pollVotes);
	}
}
