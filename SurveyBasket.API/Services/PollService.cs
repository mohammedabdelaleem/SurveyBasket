
namespace SurveyBasket.API.Services;

public class PollService : IPollService
{

	// Memory At First => DB Later 
	private static readonly List<Poll> _polls = [

		new Poll{
		Id = 1,
		Title = "Title 1",
		Description = "Desription 1"
		}
		,new Poll{
		Id = 2,
		Title = "Title 2",
		Description = "Desription 2"
		}
		,
		new Poll{
		Id = 3,
		Title = "Title 3",
		Description = "Desription 3"
		}
		,
		new Poll{
		Id = 4,
		Title = "Title 4",
		Description = "Desription 4"
		}

		];

	public Poll Add(Poll poll)
	{
		poll.Id = _polls.Count + 1;
		_polls.Add(poll);
		return poll;
	}

	public bool Delete(int id)
	{
		var poll = Get(id);

		if (poll == null) return false;

		_polls.Remove(poll);
		return true;
	}

	public Poll? Get(int id) => _polls.SingleOrDefault(poll => poll.Id == id);

	public IEnumerable<Poll> GetAll() => _polls;

	public bool Update(int id,Poll poll)
	{
		Poll? pollDB = Get(id);

		if (pollDB == null)
			return false;
		
		if(!string.IsNullOrWhiteSpace(poll.Title))
		{
			pollDB.Title = poll.Title;
		}

		if (!string.IsNullOrWhiteSpace(poll.Description))
		{
			pollDB.Description = poll.Description;
		}

		return true;
	}
}
