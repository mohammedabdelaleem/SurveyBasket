namespace SurveyBasket.API.Abstractions;

public static class ResultExtensions
{
	// Problem Returns Object Result 
	// We Need To Return Response In Failure ==> like our configuration Package 
	// rfc url , status code , errors , ...

	public static ObjectResult ToProblem(this Result result )
	{
		if (result.IsSuccess)
			throw new InvalidOperationException("Cannot Convert Success Result Into Problem Response!");

		var problem = Results.Problem(statusCode:result.Error.StatusCode);
		var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

		problemDetails!.Extensions = new Dictionary<string, object?>()
		{
			{
				"errors", new [] { 
					result.Error.Code,
				result.Error.Description
				}
			}
		};

		return new ObjectResult(problemDetails);
	}
}
