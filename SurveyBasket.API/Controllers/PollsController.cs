



using SurveyBasket.API.Contracts.Responses;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
	private readonly IPollService pollService;

	public PollsController(IPollService pollService)
	{
		this.pollService = pollService;
	}



	[HttpGet("all")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetAll()
	{
		IEnumerable<Poll> polls = pollService.GetAll();
		var response = polls.Adapt<IEnumerable<PollResponse>>();

		return (response.Count() == 0) ? NotFound(new
		{
			message = $"No Poll Yet"
		}) : Ok(response);
	}



	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Poll> Get([FromRoute] int id)
	{
		Poll? poll = pollService.Get(id);

		if (poll is null)
			return NotFound(new { message = $"Poll With {id} Not Found" });

		// source.Adapt<destination>()
		var response = poll.Adapt<PollResponse>();
		return Ok(response);
	}


	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public ActionResult Add([FromBody] PollRequest poll)
	{
		Poll newPoll = pollService.Add(poll.Adapt<Poll>());
		return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);

	}


	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public ActionResult Update(int id, PollRequest poll)
	{
		bool isUpdated = pollService.Update(id, poll.Adapt<Poll>());

		if (!isUpdated)
			return NotFound(new { message = $"ERROR : Poll {id} Not Updated" });

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public ActionResult Update([FromRoute] int id)
	{
		bool isDeleted = pollService.Delete(id);

		if (!isDeleted)
			return NotFound(new { message = $"ERROR : Poll {id} Not Deleted" });

		return NoContent();
	}


	
}

