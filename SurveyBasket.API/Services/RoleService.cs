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
}
