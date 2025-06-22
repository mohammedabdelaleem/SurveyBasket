namespace SurveyBasket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController(IRoleService roleService) : ControllerBase
{
	private readonly IRoleService _roleService = roleService;

	[HttpGet]
	[HasPermission(Permissions.GetRoles)]
	public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled,CancellationToken cancellationToken=default)
	{
		var result = await _roleService.GetAllAsync(includeDisabled, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpGet("{id}")]
	[HasPermission(Permissions.GetRoles)]
	public async Task<IActionResult> Get([FromRoute] string id)
	{
		var result = await _roleService.GetAsync(id);
		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}
}
