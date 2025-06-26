using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.API.Contracts.Common;
using SurveyBasket.API.Contracts.Votes;

namespace SurveyBasket.API.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize(Roles = DefaultRoles.Member)]
[EnableRateLimiting(RateLimiterInfo.UserAddressPolicy)]
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]

public class VotesController(IQuestionService questionService,IVoteService voteService) : ControllerBase
{
	private readonly IQuestionService _questionService = questionService;
	private readonly IVoteService _voteService = voteService;

	[HttpGet]
	public async Task<IActionResult> Start([FromRoute] int pollId, [FromQuery] RequestFilters filters ,CancellationToken cancellation=default)
	{
		// Authorized User At HttpContext
		string userId = User.GetUserId()!;

		var result = await _questionService.GetAvailableAsync(pollId, userId, filters, cancellation);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpPost]
	public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request ,CancellationToken cancellation = default)
	{
		var result = await _voteService.AddVoteAsync(pollId, User.GetUserId()!, request, cancellation);

		return result.IsSuccess ? Created() : result.ToProblem();
	}
}
