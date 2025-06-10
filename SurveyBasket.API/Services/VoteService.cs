using SurveyBasket.API.Contracts.Questions;
using SurveyBasket.API.Contracts.Votes;
using System.Linq.Expressions;

namespace SurveyBasket.API.Services;

public class VoteService(AppDbContext context ,ILogger<VoteService> logger) : IVoteService
{
	private readonly AppDbContext _context = context;
	private readonly ILogger<VoteService> _logger = logger;

	public async Task<Result> AddVoteAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
	{

		try{

			var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId &&
			p.IsPublished &&
			DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt &&
			DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt,
		cancellationToken);

		if (!pollIsExists)
			return Result.Failure(PollErrors.PollNotFound);


		var userIsExists = await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
		if (!userIsExists)
			return Result.Failure(UserErrors.UserNotFound);

		// before adding new vote to db , are there any votes at db with the same userId and PollId
		var isVoted = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
		if (isVoted)
			return Result.Failure(VoteErrors.DuplicateVote);


		var availableQuestionsIds = await _context.Questions
			.Where(q=>q.PollId == pollId && q.IsActive)
			.Select(q => q.Id)
			.ToListAsync(cancellationToken);

		var questionsIdsFromRequest = request.Answers.Select(x=>x.QuestionId).ToList();
		
		var equalQuestionsSequences = availableQuestionsIds.SequenceEqual(questionsIdsFromRequest);

		if (!equalQuestionsSequences)
			return Result.Failure(VoteErrors.InvalidQuestions);


		Vote newVote = new Vote
		{
			PollId = pollId,
			UserId = userId,
			VoteAnswers = request.Answers.Adapt<ICollection<VoteAnswer>>(),
		};

		await _context.AddAsync(newVote,cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success();
		
		}
		catch(Exception ex)
		{
			_logger.LogError(ex.Message);
			return Result.Failure(QuestionErrors.SaveError);
		}
	}
}
