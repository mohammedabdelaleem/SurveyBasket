namespace SurveyBasket.API.Errors;


public static class PollErrors
{

	public static readonly Error PollNotFound = new Error("Poll.NotFound", "Poll With Given Id Not Found", StatusCodes.Status404NotFound);
	public static readonly Error PollsEmpty = new Error("Polls.Empty", "No Polls Are Found", StatusCodes.Status404NotFound);
	public static readonly Error SaveError = new Error("Poll.SaveError", "Error While Saving", StatusCodes.Status500InternalServerError
		);



}
