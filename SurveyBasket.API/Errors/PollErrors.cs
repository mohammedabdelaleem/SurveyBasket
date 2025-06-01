namespace SurveyBasket.API.Errors;


public static class PollErrors
{

	public static readonly Error PollNotFound = new Error("Poll.NotFound", "Poll With Given Id Not Found");
	public static readonly Error PollsEmpty = new Error("Polls.Empty", "No Polls Are Found");
	public static readonly Error SaveError = new Error("Poll.SaveError", "Error While Saving");



}
