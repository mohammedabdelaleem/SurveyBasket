using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Services;

public interface IUserService
{
	Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId);
	Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
}
