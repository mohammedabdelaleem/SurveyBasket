using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Services;

public interface IUserService
{
	Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
	Task <Result<UserProfileResponse>> GetUserProfileAsync(string userId);
	Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
	Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}
