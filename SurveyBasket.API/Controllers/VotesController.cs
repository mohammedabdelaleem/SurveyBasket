

namespace SurveyBasket.API.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize]

public class VotesController(IQuestionService questionService) : ControllerBase
{
	private readonly IQuestionService _questionService = questionService;

	[HttpGet]
	public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellation=default)
	{
		// Authorized User At HttpContext
		string userId = User.GetUserId()!;

		var result = await _questionService.GetAvailableAsync(pollId, userId, cancellation);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}
}
