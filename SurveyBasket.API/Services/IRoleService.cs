using SurveyBasket.API.Contracts.Roles;

namespace SurveyBasket.API.Services;

public interface IRoleService
{
	Task<Result<IEnumerable<RoleResponse>>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);

	Task<Result<RoleDetailsResponse>> GetAsync(string roleId, CancellationToken cancellationToken = default);
}
