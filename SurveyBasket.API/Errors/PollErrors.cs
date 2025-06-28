namespace SurveyBasket.API.Errors;


public record PollErrors
{

	public static readonly Error PollNotFound
		= new Error("Poll.NotFound", "Poll With Given Id Not Found", StatusCodes.Status404NotFound);


	//public static readonly Error AccessDenied
	//= new Error("Poll.AccessDenied", "Can't Make Votes On This Poll , You Did Before", StatusCodes.Status404NotFound);


	public static readonly Error PollsEmpty
		= new Error("Polls.Empty", "No Polls Are Found", StatusCodes.Status404NotFound);


	public static readonly Error SaveError
		= new Error("Poll.SaveError", "Error While Saving", StatusCodes.Status500InternalServerError);


	public static readonly Error DuplicateTitle
		= new Error("Poll.DuplicateTitle", "Title With The Same Entered Value Is Found , Please Enter Another One", StatusCodes.Status409Conflict);




}
