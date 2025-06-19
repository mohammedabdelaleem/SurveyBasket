using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
	private readonly UserManager<ApplicationUser> _userManager = userManager;

	public async Task<Result<UserProfileResponse>> GetUserProfileInfo(string userId)
	{
		//var user = await _userManager.FindByIdAsync(userId); // don't user this technique , it select all columns , use joins & unuseful staff , and we need projection 

		var user = await _userManager.Users
			.Where(u => u.Id == userId)
			.ProjectToType<UserProfileResponse>()
			.SingleAsync();

		return Result.Success(user);
	}
}
