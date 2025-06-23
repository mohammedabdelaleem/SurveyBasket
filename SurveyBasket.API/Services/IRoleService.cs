using SurveyBasket.API.Contracts.Roles;

namespace SurveyBasket.API.Services;

public interface IRoleService
{
	Task<Result<IEnumerable<RoleResponse>>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);
	Task<Result<RoleDetailsResponse>> GetAsync(string roleId, CancellationToken cancellationToken = default);
	Task<Result<RoleDetailsResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken = default);
	Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default);

}
