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

	[HttpGet("all")]
	public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken=default)
	{
		var result = await _questionService.GetAllAsync(pollId, cancellationToken);

		return result.IsSuccess ? Ok(result.Value)
			: result.ToProblem();
	}

	[HttpGet("{questionId}")]
	public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute]int questionId, CancellationToken cancellationToken = default)
	{
		var result = await _questionService.GetAsync(pollId, questionId, cancellationToken);

		return result.IsSuccess ? 
			Ok(result.Value) : result.ToProblem();
	}


	[HttpPost]
	public async Task<IActionResult> AddNew([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellation = default)
	{
		var result = await _questionService.AddAsync(pollId, request, cancellation);

		return result.IsSuccess ?
			CreatedAtAction(nameof(Get), new {  pollId, result.Value.Id }, result.Value) :
			result.ToProblem();
	}


	[HttpPut("{questionId}")]
	public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int questionId, [FromBody] QuestionRequest request, CancellationToken cancellation = default)
	{
		var result =await  _questionService.UpdateAsync(pollId,questionId,request, cancellation);

		return result.IsSuccess ?
			NoContent() : result.ToProblem();
	}

	[HttpPut("{questionId}/toggle-status")]
	public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellation = default)
	{
		var result = await _questionService.ToggleStatusAsync(pollId, questionId, cancellation);

		return result.IsSuccess ? 
			NoContent() : result.ToProblem();
	}
}
