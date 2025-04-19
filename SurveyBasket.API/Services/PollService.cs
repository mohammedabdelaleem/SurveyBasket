
using SurveyBasket.API.Entities;

namespace SurveyBasket.API.Services;

public class PollService(AppDbContext _context) : IPollService
{
	private readonly AppDbContext context = _context;

	public async Task< IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) => 
		await context.Polls.AsNoTracking().ToListAsync(cancellationToken);


	public async Task<Poll?> GetAsync(int id, CancellationToken cancellationToken = default) => 
		await context.Polls.FindAsync(id,cancellationToken);



	public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
	{
		await context.Polls.AddAsync(poll,cancellationToken);
		await context.SaveChangesAsync(cancellationToken);
		return poll;
	}

	public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
	{
		Poll? pollDB = await GetAsync(id, cancellationToken);

		if (pollDB == null)
			return false;

		// Update Fields
		pollDB.Title = poll.Title;
		pollDB.Summary = poll.Summary;
		//pollDB.IsPublished = poll.IsPublished;
		pollDB.StartsAt = poll.StartsAt;
		pollDB.EndsAt = poll.EndsAt;

		await context.SaveChangesAsync(cancellationToken);
		return true;
	}
	public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var poll = await GetAsync(id, cancellationToken);

		if (poll == null) return false;

		context.Remove(poll);
		await context.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
	{
		Poll? poll = await GetAsync(id, cancellationToken);

		if (poll == null)
			return false;

		poll.IsPublished = !poll.IsPublished;
		

		await context.SaveChangesAsync(cancellationToken);
		return true;
	}
}
