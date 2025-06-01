
namespace SurveyBasket.API.Errors;

public static class UserErrors
{
	public static readonly Error InvalidCredintials = new Error("User.InvalidCredintials", "Invalid Email Or Password");

	public static readonly Error UserNotFound = new Error("User.NotFound", "User Not Found");

	public static readonly Error RefreshTokenNotFound = new Error("UserRefreshToken.NotFound", "User Don't Have This RefreshToken");


}
