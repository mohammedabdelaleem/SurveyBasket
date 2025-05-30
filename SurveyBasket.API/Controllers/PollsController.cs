using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

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
	[ProducesResponseType(StatusCodes.Status200OK)] // ProducesResponseType to prevent Undocumented Endpoint
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize]
	public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
	{
		IEnumerable<Poll> polls = await pollService.GetAllAsync(cancellationToken);
		var response = polls.Adapt<IEnumerable<PollResponse>>();

		return (response.Count() == 0) ? NotFound(new
		{
			message = $"No Poll Yet"
		}) : Ok(response);
	}



	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<Poll>> Get([FromRoute] int id)
	{
		Poll? poll = await pollService.GetAsync(id);

		if (poll is null)
			return NotFound(new { message = $"Poll With {id} Not Found" });

		// source.Adapt<destination>()
		var response = poll.Adapt<PollResponse>();
		return Ok(response);
	}


	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task< ActionResult> Add([FromBody] PollRequest poll)
	{
		if (poll is null)
			return BadRequest(poll);

		//if(poll.id > 0) // id is auto generated
		//{
		//	return StatusCode(StatusCodes.Status500InternalServerError);
		//}

		Poll newPoll = await pollService.AddAsync(poll.Adapt<Poll>());
		return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);  
	}		


	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult> Update(int id, PollRequest poll, CancellationToken cancellationToken=default)
	{
		bool isUpdated = await pollService.UpdateAsync(id, poll.Adapt<Poll>(), cancellationToken);

		if (!isUpdated)
			return NotFound(new { message = $"ERROR : Poll {id} Not Updated" });

		return NoContent();
	}

	[HttpPatch("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> PartialUpdate(int id, JsonPatchDocument<PollRequest> patchDTO, CancellationToken cancellationToken = default)
	{
		if (patchDTO is null || id <= 0)
		{
			return BadRequest("Invalid patch document or ID.");
		}

		var poll = await pollService.GetAsync(id, cancellationToken);
		if (poll == null)
		{
			return NotFound(new { message = $"Poll with {nameof(id)} {id} not found" });
		}

		var pollToPatch = poll.Adapt<PollRequest>();

		patchDTO.ApplyTo(pollToPatch, ModelState);

		if (!ModelState.IsValid || !TryValidateModel(pollToPatch))
		{
			return ValidationProblem(ModelState);
		}

		pollToPatch.Adapt(poll); // Map back patched data to entity

		await pollService.UpdateAsync(id, poll, cancellationToken);

		return NoContent();
	}



	[HttpPut("{id}/togglePublish")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]

	public async Task<ActionResult> TogglePublish(int id, CancellationToken cancellationToken = default)
	{
		bool isUpdated = await pollService.TogglePublishStatusAsync(id,  cancellationToken);

		if (!isUpdated)
			return NotFound(new { message = $"ERROR : Poll {id} Not Updated" });

		return NoContent();
	}


	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)] 

	public async Task<ActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
	{
		bool isDeleted = await pollService.DeleteAsync(id, cancellationToken);

		if (!isDeleted)
			return NotFound(new { message = $"ERROR : Poll {id} Not Deleted" });

		return NoContent();
	}



}

