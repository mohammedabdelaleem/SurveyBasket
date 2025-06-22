using SurveyBasket.API.Contracts.Roles;

namespace SurveyBasket.API.Services;

public interface IRoleService
{
	Task<Result<IEnumerable<RoleResponse>>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);
}
