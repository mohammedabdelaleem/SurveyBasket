namespace SurveyBasket.API.Errors;

public class TokenErrors
{
	public static Error InvalidToken = new Error("Token.InvalidToken", "We Can't Extract User Id From Token", StatusCodes.Status404NotFound);


}
