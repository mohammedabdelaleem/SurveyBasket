namespace SurveyBasket.API.Errors;

public record TokenErrors
{
	public static Error InvalidToken = new Error("Token.InvalidToken", "We Can't Extract User Id From Token", StatusCodes.Status401Unauthorized);


}
