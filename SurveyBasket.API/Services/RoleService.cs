using SurveyBasket.API.Contracts.Roles;

namespace SurveyBasket.API.Services;

public class RoleService(
	RoleManager<ApplicationRole> roleManager,
	AppDbContext context
	) : IRoleService
{
	private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
	private readonly AppDbContext _context = context;

	public async Task<Result<IEnumerable<RoleResponse>>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken=default)
	{
		
			var roles = await _roleManager.Roles
			.Where(r => !r.IsDefault &&
					  (!r.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))  // (includeDisabled.HasValue && includeDisabled.Value) ==== (includeDisabled==true)
			.ProjectToType<RoleResponse>()
			.ToListAsync(cancellationToken);

			return Result.Success<IEnumerable<RoleResponse>>(roles);
	}


	public async Task<Result<RoleDetailsResponse>> GetAsync(string roleId, CancellationToken cancellationToken = default)
	{
		if (await _roleManager.FindByIdAsync(roleId) is not { } role)
			return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);

		var permissions = await _roleManager.GetClaimsAsync(role);

		var roleDetails = new RoleDetailsResponse(
				Id: roleId,
				Name: role.Name!,
				IsDeleted: role.IsDeleted,
				Permissions: permissions.Select(x => x.Value)
			);

		return Result.Success(roleDetails);
	}

	public async Task<Result<RoleDetailsResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken=default)
	{

		// check for Duplicate titles
		//var roleIsExists = await _roleManager.Roles.AnyAsync(r=>r.Name == request.Name, cancellationToken);
		var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);
		if(roleIsExists)
			return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleDuplicated);


		// check for: not allowed permissions xxx
		var allowedPermissions = Permissions.GetAllPermissions();
		if(request.Permissions.Except(allowedPermissions).Any())
			return Result.Failure<RoleDetailsResponse>(RoleErrors.InvalidPermissions);


		var role = new ApplicationRole
		{
			Name = request.Name,
			ConcurrencyStamp = Guid.NewGuid().ToString()
		};

		var result = await _roleManager.CreateAsync(role);

		if(result.Succeeded)
		{
			// convert from list<string> into list<IdentityRoleClaim<string>>
			var permissions = request.Permissions
				.Select(p => new IdentityRoleClaim<string>
				{
					ClaimType = Permissions.Type,
					ClaimValue = p,
					RoleId = role.Id
				});

			await _context.AddRangeAsync(permissions,cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			var roleDetails = new RoleDetailsResponse(
				Id: role.Id,
				Name: role.Name!,
				IsDeleted: role.IsDeleted,
				Permissions: request.Permissions
			);

			return Result.Success(roleDetails);

		}

		var error = result.Errors.First();
		return Result.Failure<RoleDetailsResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
	}









}
