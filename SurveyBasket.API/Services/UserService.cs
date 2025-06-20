using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
	private readonly UserManager<ApplicationUser> _userManager = userManager;

	public async Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId)
	{
		//var user = await _userManager.FindByIdAsync(userId); // don't user this technique , it select all columns , use joins & unuseful staff , and we need projection 

		var user = await _userManager.Users
			.Where(u => u.Id == userId)
			.ProjectToType<UserProfileResponse>()
			.SingleAsync();

		return Result.Success(user);
	}


	public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
	{
		var user = await _userManager.FindByIdAsync(userId);

		user = request.Adapt(user);
		await _userManager.UpdateAsync(user!);
		return Result.Success();
	}

	public async Task<Result> ChangePasswordAsync(string  userId, ChangePasswordRequest request)
	{
		var user = await _userManager.FindByIdAsync(userId);

		var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

		if (result.Succeeded)
			return Result.Success();

		var error = result.Errors.First();
		return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
	}
}
