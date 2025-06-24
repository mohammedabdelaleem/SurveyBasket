using SurveyBasket.API.Contracts.Users;

namespace SurveyBasket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
	private readonly IUserService _userService = userService;

	[HttpGet]
	[HasPermission(Permissions.GetUsers)]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken=default)
	{
		return Ok(await _userService.GetAllAsync(cancellationToken));
	}

	[HttpGet("{id}")]
	[HasPermission(Permissions.GetUsers)]
	public async Task<IActionResult> Get(string id)
	{
		var result = await _userService.GetAsync(id);
		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpPost]
	[HasPermission(Permissions.AddUser)]
	public async Task<IActionResult> Add([FromBody] CreateUserRequest request,CancellationToken cancellationToken=default)
	{
		var result = await _userService.AddAsync(request, cancellationToken);
		return result.IsSuccess ? CreatedAtAction(nameof(Get), new {result.Value.Id} , result.Value) : result.ToProblem();
	}
}
