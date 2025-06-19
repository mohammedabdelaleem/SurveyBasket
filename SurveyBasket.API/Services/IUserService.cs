using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Services;

public interface IUserService
{
	Task<Result<UserProfileResponse>> GetUserProfileInfo(string userId);
}
