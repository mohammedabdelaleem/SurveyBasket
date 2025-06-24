using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Services;

public interface IUserService
{
	Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<Result<UserResponse>> GetAsync(string userId);
	Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
	Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
	Task<Result> ToggleStatus(string id);
	Task<Result> UnLock(string id);
	Task <Result<UserProfileResponse>> GetUserProfileAsync(string userId);
	Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
	Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}
