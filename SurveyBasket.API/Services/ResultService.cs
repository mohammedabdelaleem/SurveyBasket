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


	public async Task<Result<IEnumerable<VotesPerDayRespnse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
	{
		var pollIsExists = await _context.Polls.AnyAsync(p=>p.Id==pollId , cancellationToken);
		if (!pollIsExists)
			return Result.Failure<IEnumerable<VotesPerDayRespnse>>(PollErrors.PollNotFound);


		var votesPerDay = await _context.Votes
			.Where(v => v.PollId == pollId)
			.GroupBy(v => new { Date = DateOnly.FromDateTime(v.SubmittedOn) })
			.Select(g => new VotesPerDayRespnse(
				g.Key.Date,
				g.Count()))
			.ToListAsync(cancellationToken);

		return Result.Success<IEnumerable<VotesPerDayRespnse>>(votesPerDay);

	}


	public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
	{
		var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
		if (!pollIsExists)
			return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);


	var votesPerQuestion = await _context.VoteAnswers
			.Where(v=>v.Vote.PollId == pollId)
			.Select(v=> new VotesPerQuestionResponse (
				v.Question.Content,
				v.Question.Votes
					.GroupBy(v=> new {AnswerId = v.Answer.Id , AnswerContent = v.Answer.Content })
					.Select(g=> new VotesPerAnswerResponse(
						g.Key.AnswerContent,
						g.Count())
				)))
			.ToListAsync(cancellationToken);


		return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);

	}

}
