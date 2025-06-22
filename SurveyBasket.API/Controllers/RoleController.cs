using SurveyBasket.API.Contracts.Roles;

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

	[HttpPost]
	[HasPermission(Permissions.AddRole)]
	public async Task<IActionResult> Add([FromBody] RoleRequest request, CancellationToken cancellationToken = default)
	{
		var result = await _roleService.AddAsync(request,cancellationToken);
		return result.IsSuccess ? CreatedAtAction(nameof(Get) , new { id=result.Value.Id}, result.Value) : result.ToProblem();
	}
}
