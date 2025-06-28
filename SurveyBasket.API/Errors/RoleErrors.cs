namespace SurveyBasket.API.Errors;


public record RoleErrors
{

	public static readonly Error RoleNotFound 
		= new Error("Role.NotFound", "Role With Given Id Not Found", StatusCodes.Status404NotFound);


	public static readonly Error RoleDuplicated
		= new Error("Role.Duplicated", "Role With The Same Name Is Already Exists", StatusCodes.Status409Conflict);

	public static readonly Error InvalidPermissions
	= new Error("Role.InvalidPermissions", "Invalid Permissions", StatusCodes.Status400BadRequest);


}
