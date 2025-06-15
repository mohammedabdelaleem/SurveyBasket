
namespace SurveyBasket.API.Errors;

public static class UserErrors
{
	public static readonly Error InvalidCredintials = new Error("User.InvalidCredintials", "Invalid Email Or Password", StatusCodes.Status404NotFound);

	public static readonly Error UserNotFound = new Error("User.NotFound", "User Not Found", StatusCodes.Status404NotFound);

	public static readonly Error RefreshTokenNotFound = new Error("UserRefreshToken.NotFound", "User Don't Have This RefreshToken", StatusCodes.Status404NotFound);

	public static readonly Error DuplicatedEmail = new Error("User.DuplicatedEmail", "Email With The Same Value Is Exists", StatusCodes.Status409Conflict);

}
