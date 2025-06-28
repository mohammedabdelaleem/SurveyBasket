namespace SurveyBasket.API.Errors;


public record VoteErrors
{

	public static readonly Error PollNotFound
		= new Error("Vote.NotFound", "Vote With Given Id Not Found", StatusCodes.Status404NotFound);


	public static readonly Error SaveError
		= new Error("Vote.SaveError", "Error While Saving", StatusCodes.Status500InternalServerError);


	public static readonly Error DuplicateVote
		= new Error("Vote.DuplicateVote", "This User Is Voted Before,You Can Vote Once", StatusCodes.Status409Conflict);

	public static readonly Error InvalidQuestions
	= new Error("Question.InvalidQuestions", "Invalid Questions", StatusCodes.Status404NotFound);


}
