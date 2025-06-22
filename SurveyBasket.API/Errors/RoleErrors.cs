namespace SurveyBasket.API.Errors;


public static class RoleErrors
{

	public static readonly Error RoleNotFound 
		= new Error("Role.NotFound", "Role With Given Id Not Found", StatusCodes.Status404NotFound);
	

}
