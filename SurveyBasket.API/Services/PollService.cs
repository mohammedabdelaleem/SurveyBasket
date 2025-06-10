

namespace SurveyBasket.API.Services;

public class PollService(AppDbContext _context) : IPollService
{
	private readonly AppDbContext context = _context;

	public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var polls = await context.Polls
			.AsNoTracking()
			.ProjectToType<PollResponse>()
			.ToListAsync(cancellationToken);

		return (polls.Count() > 0) ?
			Result.Success<IEnumerable<PollResponse>>(polls) :
			Result.Failure<IEnumerable<PollResponse>>(PollErrors.PollsEmpty);
	}
	public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
	{
		// which polls i can choose to make votes on it ?
		var polls = await context.Polls
			.Where(p=>p.IsPublished && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt )
			.AsNoTracking()
			.ProjectToType<PollResponse>()
			.ToListAsync(cancellationToken);

		return (polls.Count() > 0) ?
			Result.Success<IEnumerable<PollResponse>>(polls) :
			Result.Failure<IEnumerable<PollResponse>>(PollErrors.PollsEmpty);
	}
	public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
	{
		var poll =await context.Polls.FindAsync(id,cancellationToken);
		return (poll != null) ?
			Result.Success(poll.Adapt<PollResponse>()) :
			Result.Failure<PollResponse>(PollErrors.PollNotFound);
	}

	public async Task<Result<PollResponse>> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken = default)
	{

		bool isExistingTitle = await _context.Polls.AnyAsync(p => p.Title == pollRequest.Title, cancellationToken: cancellationToken);
		if (isExistingTitle)
			return Result.Failure<PollResponse>(PollErrors.DuplicateTitle);

		Poll poll = pollRequest.Adapt<Poll>();
		await context.Polls.AddAsync(poll, cancellationToken);
		int numberOfStates=await context.SaveChangesAsync(cancellationToken);

		var pollResponse = poll.Adapt<PollResponse>();

		return numberOfStates!=0 ? Result.Success(pollResponse) : Result.Failure<PollResponse>(PollErrors.SaveError);
	}

	public async Task<Result> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken = default)
	{

		bool isExistingTitle = await _context.Polls.AnyAsync(p => p.Title == pollRequest.Title && p.Id != id, cancellationToken: cancellationToken);
		if (isExistingTitle)
			return Result.Failure<PollResponse>(PollErrors.DuplicateTitle);



		Poll? pollDB = await context.Polls.FindAsync(id, cancellationToken);

		if (pollDB == null)
			return Result.Failure(PollErrors.PollNotFound);

		// Update Fields
		pollRequest.Adapt<Poll>();
		pollRequest.Adapt(pollDB);

		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
	public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var poll = await context.Polls.FindAsync(id, cancellationToken);

		if (poll is null)return Result.Failure(PollErrors.PollNotFound);
		
		context.Remove(poll);

		int numberOfStates = await context.SaveChangesAsync(cancellationToken);

		return numberOfStates != 0 ? Result.Success() : Result.Failure(PollErrors.SaveError);
	}

	public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
	{
		Poll? poll = await context.Polls.FindAsync(id, cancellationToken);

		if (poll == null)
			if (poll == null) return Result.Failure(PollErrors.PollNotFound);

		poll.IsPublished = !poll.IsPublished;

		int numberOfStates = await context.SaveChangesAsync(cancellationToken);
		return numberOfStates != 0 ? Result.Success() : Result.Failure(PollErrors.SaveError);
	}
}
