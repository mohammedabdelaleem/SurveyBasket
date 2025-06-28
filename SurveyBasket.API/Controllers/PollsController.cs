using Asp.Versioning;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.RateLimiting;

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[ApiVersion(1, Deprecated =true)]
[ApiVersion(2)]

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
	[HasPermission(Permissions.GetPolls)]
	public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
	{
		var result = await pollService.GetAllAsync(cancellationToken);

		return (result.IsFailure) ? 
			result.ToProblem()
			: Ok(result.Value);

		#region Problem => Results
			//{
			//	"type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
			//	"title": "Poll.NotFound",
			//	"status": 404,
			//	"detail": "Poll With Given Id Not Found",
			//	"traceId": "00-d5b10ee08cf178f88fd9e72d1dba5fed-e7266b5b09d3e5d6-00"
			//}
		#endregion

	}


	[MapToApiVersion(1)]
	[HttpGet("current")]
	[ProducesResponseType(StatusCodes.Status200OK)] // ProducesResponseType to prevent Undocumented Endpoint
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = DefaultRoles.Member)]
	[EnableRateLimiting(RateLimiterInfo.IpAddressPolicy)]
	public async Task<ActionResult> GetCurrentV1(CancellationToken cancellationToken)
	{
		var result = await pollService.GetCurrentAsyncV1(cancellationToken);

		return (result.IsFailure) ?
			result.ToProblem()
			: Ok(result.Value);

	}


	[MapToApiVersion(2)]
	[HttpGet("current")]
	[ProducesResponseType(StatusCodes.Status200OK)] // ProducesResponseType to prevent Undocumented Endpoint
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = DefaultRoles.Member)]
	[EnableRateLimiting(RateLimiterInfo.IpAddressPolicy)]
	public async Task<ActionResult> GetCurrentV2(CancellationToken cancellationToken)
	{
		var result = await pollService.GetCurrentAsyncV2(cancellationToken);

		return (result.IsFailure) ?
			result.ToProblem()
			: Ok(result.Value);

	}



	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[HasPermission(Permissions.GetPolls)]
	public async Task<ActionResult<Poll>> Get([FromRoute] int id)
	{
		var result = await pollService.GetAsync(id);
		return (result.IsSuccess) ? Ok(result.Value) :
			result.ToProblem();
	}


	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[HasPermission(Permissions.AddPoll)]
	public async Task<ActionResult> Add([FromBody] PollRequest poll, CancellationToken cancellationToken=default)
	{

		if (poll is null)
			return BadRequest(poll);

		var result = await pollService.AddAsync(poll, cancellationToken);

		return (result.IsSuccess)? 
		 CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value):
		 result.ToProblem();
	}


	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[HasPermission(Permissions.UpdatePoll)]
	public async Task<ActionResult> Update([FromRoute]int id, [FromBody] PollRequest poll, CancellationToken cancellationToken = default)
	{
		var result = await pollService.UpdateAsync(id, poll, cancellationToken);

		return(result.IsSuccess)? NoContent():
		 result.ToProblem();

	}


	[HttpPatch("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HasPermission(Permissions.UpdatePoll)]
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

		await pollService.UpdateAsync(id, poll.Adapt<PollRequest>(), cancellationToken);

		return NoContent();
	}



	[HttpPut("{id}/togglePublish")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[HasPermission(Permissions.UpdatePoll)]
	public async Task<ActionResult> TogglePublish(int id, CancellationToken cancellationToken = default)
	{
		var result = await pollService.TogglePublishStatusAsync(id, cancellationToken);

		return (result.IsSuccess)? NoContent():
			 result.ToProblem();
	}


	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[HasPermission(Permissions.DeletePoll)]
	public async Task<ActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
	{
		var result = await pollService.DeleteAsync(id, cancellationToken);

		return (result.IsSuccess) ? NoContent() :
			 result.ToProblem();
	}
}

