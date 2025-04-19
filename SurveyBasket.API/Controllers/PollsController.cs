﻿

using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
		if (patchDTO is null || id == 0)
		{
			return BadRequest();
		}

		// Fetch the existing poll entity
		var poll = await pollService.GetAsync(id, cancellationToken);
		if (poll == null)
		{
			return NotFound(new { message = $"Poll with ID {id} not found" });
		}

		// Adapt the entity to a DTO that can be patched
		var pollToPatch = poll.Adapt<PollRequest>();

		// Apply the patch
		patchDTO.ApplyTo(pollToPatch, ModelState);

		// Check if the patch resulted in invalid model state
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		// Map the patched DTO back to the entity
		pollToPatch.Adapt(poll);

		// Save changes to the database
		await pollService.UpdateAsync(id, poll, cancellationToken);

		// Return success with no content
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

