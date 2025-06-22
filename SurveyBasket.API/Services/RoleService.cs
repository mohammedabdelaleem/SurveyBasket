using SurveyBasket.API.Contracts.Roles;

namespace SurveyBasket.API.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
	private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

	public async Task<Result<IEnumerable<RoleResponse>>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken=default)
	{
		
			var roles = await _roleManager.Roles
			.Where(r => !r.IsDefault &&
					  (!r.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))  // (includeDisabled.HasValue && includeDisabled.Value) ==== (includeDisabled==true)
			.ProjectToType<RoleResponse>()
			.ToListAsync(cancellationToken);

			return Result.Success<IEnumerable<RoleResponse>>(roles);
	}


	public async Task<Result<RoleDetailsResponse>> GetAsync(string roleId, CancellationToken cancellationToken = default)
	{
		if (await _roleManager.FindByIdAsync(roleId) is not { } role)
			return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);

		var permissions = await _roleManager.GetClaimsAsync(role);

		var roleDetails = new RoleDetailsResponse(
				Id: roleId,
				Name: role.Name!,
				IsDeleted: role.IsDeleted,
				Permissions: permissions.Select(x => x.Value)
			);

		return Result.Success(roleDetails);
	}








}
