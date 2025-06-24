using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Services;

public class UserService(
	UserManager<ApplicationUser> userManager,
	AppDbContext context) : IUserService
{
	private readonly UserManager<ApplicationUser> _userManager = userManager;
	private readonly AppDbContext _context = context;

	public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)=>
		await (
			from u in _context.Users
			join ur in _context.UserRoles
			on u.Id equals ur.UserId
			join r in _context.Roles
			on ur.RoleId equals r.Id into roles // "Group all the roles for each user into a list, don’t repeat the user for each role."  === This is a group join, which creates a list of roles per user.
			where !roles.Any(r => r.Name == DefaultRoles.Member) 
			select new // what data you need from tables  
			{
				u.Id,
				u.FirstName,
				u.LastName,
				u.Email,
				u.IsDisabled,
				Roles = roles.Select(r => r.Name).ToList()
			}
			)
			.GroupBy(u => new // grouping for remove duplication
			{
				u.Id,
				u.FirstName,
				u.LastName,
				u.Email,
				u.IsDisabled
			})
			.Select(u => new UserResponse(
					u.Key.Id,
					u.Key.FirstName,
					u.Key.LastName,
					u.Key.Email,
					u.Key.IsDisabled,
					u.SelectMany(x => x.Roles)
			))
			.ToListAsync(cancellationToken);

	public async Task<Result<UserResponse>> GetAsync(string userId)
	{
		if(await _userManager.FindByIdAsync(userId) is not { } user)
			return Result.Failure<UserResponse>(UserErrors.UserNotFound);

		var userRoles = await _userManager.GetRolesAsync(user);

		var response = (user, userRoles).Adapt<UserResponse>();

		return Result.Success(response);
	}


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
		// Bad Performance => select user with its refresh tokens , update all properties even if we need only 2 columns
		//var user = await _userManager.FindByIdAsync(userId);
		//user = request.Adapt(user);
		//await _userManager.UpdateAsync(user!);

		await _userManager.Users
			.Where(u => u.Id == userId)
			.ExecuteUpdateAsync(setters =>
				setters
					.SetProperty(u => u.FirstName, request.FirstName)
					.SetProperty(u => u.LastName, request.LastName)
					);

		return Result.Success();
	}

	public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
	{
		var user = await _userManager.FindByIdAsync(userId);

		var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

		if (result.Succeeded)
			return Result.Success();

		var error = result.Errors.First();
		return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
	}
}
