
using Hangfire;

namespace SurveyBasket.API.Services;

public class PollService(
	AppDbContext context,
	INotificationService notificationService1
	) : IPollService
{
	private readonly AppDbContext _context = context;
	private readonly INotificationService _notificationService1 = notificationService1;

	public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var polls = await _context.Polls
			.AsNoTracking()
			.ProjectToType<PollResponse>()
			.ToListAsync(cancellationToken);

		return (polls.Count() > 0) ?
			Result.Success<IEnumerable<PollResponse>>(polls) :
			Result.Failure<IEnumerable<PollResponse>>(PollErrors.PollsEmpty);
	}
	public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsyncV1(CancellationToken cancellationToken = default)
	{
		// which polls i can choose to make votes on it ?
		var polls = await _context.Polls
			.Where(p => p.IsPublished && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt)
			.AsNoTracking()
			.ProjectToType<PollResponse>()
			.ToListAsync(cancellationToken);

		return (polls.Count() > 0) ?
			Result.Success<IEnumerable<PollResponse>>(polls) :
			Result.Failure<IEnumerable<PollResponse>>(PollErrors.PollsEmpty);
	}


	public async Task<Result<IEnumerable<PollResponseV2>>> GetCurrentAsyncV2(CancellationToken cancellationToken = default)
	{
		// which polls i can choose to make votes on it ?
		var polls = await _context.Polls
			.Where(p => p.IsPublished && DateOnly.FromDateTime(DateTime.UtcNow) >= p.StartsAt && DateOnly.FromDateTime(DateTime.UtcNow) <= p.EndsAt)
			.AsNoTracking()
			.ProjectToType<PollResponseV2>()
			.ToListAsync(cancellationToken);

		return (polls.Count() > 0) ?
			Result.Success<IEnumerable<PollResponseV2>>(polls) :
			Result.Failure<IEnumerable<PollResponseV2>>(PollErrors.PollsEmpty);
	}

	public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
	{
		var poll = await _context.Polls.FindAsync(id, cancellationToken);
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
		await _context.Polls.AddAsync(poll, cancellationToken);
		int numberOfStates = await _context.SaveChangesAsync(cancellationToken);

		var pollResponse = poll.Adapt<PollResponse>();

		return numberOfStates != 0 ? Result.Success(pollResponse) : Result.Failure<PollResponse>(PollErrors.SaveError);
	}

	public async Task<Result> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken = default)
	{

		bool isExistingTitle = await _context.Polls.AnyAsync(p => p.Title == pollRequest.Title && p.Id != id, cancellationToken: cancellationToken);
		if (isExistingTitle)
			return Result.Failure<PollResponse>(PollErrors.DuplicateTitle);



		Poll? pollDB = await _context.Polls.FindAsync(id, cancellationToken);

		if (pollDB == null)
			return Result.Failure(PollErrors.PollNotFound);

		// Update Fields
		pollDB = pollRequest.Adapt(pollDB);

		await _context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
	public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var poll = await _context.Polls.FindAsync(id, cancellationToken);

		if (poll is null) return Result.Failure(PollErrors.PollNotFound);

		_context.Remove(poll);

		int numberOfStates = await _context.SaveChangesAsync(cancellationToken);

		return numberOfStates != 0 ? Result.Success() : Result.Failure(PollErrors.SaveError);
	}

	public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
	{
		Poll? poll = await _context.Polls.FindAsync(id, cancellationToken);

		if (poll == null)
			if (poll == null) return Result.Failure(PollErrors.PollNotFound);

		poll.IsPublished = !poll.IsPublished;

		int numberOfStates = await _context.SaveChangesAsync(cancellationToken);

		// here we need to send notification when 
		// date is today and admin change the status 

		if (poll.IsPublished && poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
			BackgroundJob.Enqueue(() => _notificationService1.SendNewPollsNotification(poll.Id));



		return numberOfStates != 0 ? Result.Success() : Result.Failure(PollErrors.SaveError);
	}
}


