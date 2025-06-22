

namespace SurveyBasket.API.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[HasPermission(Permissions.Results)]
public class ResultsController(IResultService resultService) : ControllerBase
{
	private readonly IResultService _resultService = resultService;

	[HttpGet("row-data")]
	public async Task<IActionResult> GetPollVotes([FromRoute] int pollId , CancellationToken cancellationToken=default)
	{
		var result = await _resultService.GetPollVotesAsync(pollId, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpGet("votes-per-day")]
	public async Task<IActionResult> GetVotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken = default)
	{
		var result = await _resultService.GetVotesPerDayAsync(pollId, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpGet("votes-per-question")]
	public async Task<IActionResult> GetVotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken = default)
	{
		var result = await _resultService.GetVotesPerQuestionAsync(pollId, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

}

