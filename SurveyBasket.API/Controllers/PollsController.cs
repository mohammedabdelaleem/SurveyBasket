


namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
	private readonly IPollService pollService;

	public PollsController(IPollService pollService )
    {
		this.pollService = pollService;
	}



    [HttpGet("all")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetAll()
	{
		IEnumerable<Poll> polls = pollService.GetAll();
		return (polls.Count() == 0 ) ? NotFound(new { message = $"No Poll Yet"}) :Ok(polls);
	}


	[HttpGet("{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<Poll> Get(int id)
	{
		Poll? poll = pollService.Get( id);
		return poll == null ? NotFound(new { message = $"Poll With {id} Not Found" }) : Ok(poll);
	}


	[HttpPost(template: "")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public ActionResult Add(Poll poll)
	{
		 Poll newPoll=  pollService.Add(poll);
		return CreatedAtAction(nameof(Get) , new { id = newPoll.Id } , newPoll);
	}

	[HttpPut("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public ActionResult Update(int id, Poll poll)
	{
		bool isUpdated = pollService.Update(id, poll);

		if (!isUpdated)
			return NotFound(new { message = $"ERROR : Poll {id} Not Updated" });

		return NoContent();
	}

	[HttpDelete("{id:int}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public ActionResult Update(int id)
	{
		bool isDeleted = pollService.Delete(id);

		if (!isDeleted)
			return NotFound(new { message = $"ERROR : Poll {id} Not Deleted" });

		return NoContent();
	}
}
