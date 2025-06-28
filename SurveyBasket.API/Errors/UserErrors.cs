
namespace SurveyBasket.API.Errors;

public record UserErrors
{
	public static readonly Error InvalidCredintials = new Error("User.InvalidCredintials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);

	public static readonly Error DisabledUser = new Error("User.DisabledUser", "Disabled User , Please Contact The Customer Service", StatusCodes.Status401Unauthorized);

	public static readonly Error LockedUser = new Error("User.LockedUser", "Locked User , Please Contact The Customer Service", StatusCodes.Status401Unauthorized);

	public static readonly Error UserNotFound = new Error("User.NotFound", "User Not Found", StatusCodes.Status404NotFound);

	public static readonly Error RefreshTokenNotFound = new Error("UserRefreshToken.NotFound", "User Don't Have This RefreshToken", StatusCodes.Status404NotFound);

	public static readonly Error DuplicatedEmail = new Error("User.DuplicatedEmail", "Email With The Same Value Is Exists", StatusCodes.Status409Conflict);

	public static readonly Error EmailNotConfirmed = new Error("User.EmailNotConfirmed", "Email Is Not Confirmed", StatusCodes.Status401Unauthorized);

	public static readonly Error InvalidCode = new Error("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);

	public static readonly Error DuplicatedConfirmation = new Error("User.DuplicatedConfirmation", "Email Already Confirmed", StatusCodes.Status400BadRequest);


	public static readonly Error InvalidRoles = new Error("User.InvalidRoles", "Invalid Roles", StatusCodes.Status400BadRequest);
}
