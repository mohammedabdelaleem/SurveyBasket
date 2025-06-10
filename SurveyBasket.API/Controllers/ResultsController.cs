namespace SurveyBasket.API.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class ResultsController(IResultService resultService) : ControllerBase
{
	private readonly IResultService _resultService = resultService;

	[HttpGet("row-data")]
	public async Task<IActionResult> GetPollVotes([FromRoute] int pollId , CancellationToken cancellationToken=default)
	{
		var result = await _resultService.GetPollVotesAsync(pollId, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}
}
