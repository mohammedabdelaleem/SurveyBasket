
namespace SurveyBasket.API.Persistance.EntitiesConfigurations;


public class RoleClaimConfigurations : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
	public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
	{
		var permissions = Permissions.GetAllPermissions();

		var adminClaims = new List<IdentityRoleClaim<string>>();

		for (int i = 0; i < permissions.Count; i++)
		{
			adminClaims.Add(new IdentityRoleClaim<string>()
			{
				Id = i + 1,
				ClaimType = Permissions.Type,
				ClaimValue = permissions[i],
				RoleId = DefaultRoles.AdminRoleId,
			});
		}

		builder.HasData(
			adminClaims
			);
	}
}
