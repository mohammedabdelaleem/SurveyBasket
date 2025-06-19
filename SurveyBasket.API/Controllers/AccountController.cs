
using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
	private readonly IUserService _userService = userService;

	[HttpGet]
	public async Task<IActionResult> Info()
	{
		var result = await _userService.GetUserProfileAsync(User.GetUserId()!);
		return Ok(result.Value);
	}


	[HttpPut("info")]
	public async Task<IActionResult> Info([FromBody] UpdateProfileRequest request)
	{
		 await _userService.UpdateProfileAsync(User.GetUserId()!, request);
		return NoContent();
	}
}
