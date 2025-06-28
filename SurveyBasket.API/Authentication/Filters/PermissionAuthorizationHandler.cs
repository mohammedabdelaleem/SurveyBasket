namespace SurveyBasket.API.Authentication.Filters;

public class PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger) : AuthorizationHandler<PermissionRequirement>
{
	private readonly ILogger<PermissionAuthorizationHandler> _logger = logger;

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
	{
		var user = context.User.Identity;
		if (user is null || !user.IsAuthenticated)
			return;


		var hasPermission = context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type);
		if (hasPermission)
		{
			context.Succeed(requirement);
			return;
		}



		//if (context.User.Identity is not { IsAuthenticated :true} ||
		//	!context.User.Claims.Any(x=>x.Value == requirement.Permission && x.Type == "permissions"))
		//	return;


		//context.Succeed(requirement);


		return;
	}
}
