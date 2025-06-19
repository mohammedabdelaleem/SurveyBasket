
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
		var result = await _userService.GetUserProfileInfo(User.GetUserId()!);

		return Ok(result.Value);
	}
}
