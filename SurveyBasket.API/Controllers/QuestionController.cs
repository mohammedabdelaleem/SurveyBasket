using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionController(IQuestionService questionService) : ControllerBase
{
	private readonly IQuestionService _questionService = questionService;

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int pollId , int id)
	{
		return  new ContentResult();
	}


	[HttpPost]
	public async Task<IActionResult> AddNew([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellation = default)
	{
		var result = await _questionService.AddAsync(pollId, request, cancellation);

		return result.IsSuccess ?
			CreatedAtAction(nameof(Get), new {  pollId, result.Value.Id }, result.Value) :
			result.ToProblem();
	}

}
