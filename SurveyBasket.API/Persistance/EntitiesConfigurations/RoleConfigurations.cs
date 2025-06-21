using SurveyBasket.API.Abstractions.Consts;

namespace SurveyBasket.API.Persistance.EntitiesConfigurations;


public class RoleConfigurations : IEntityTypeConfiguration<ApplicationRole>
{
	public void Configure(EntityTypeBuilder<ApplicationRole> builder)
	{

		builder.HasData(
			[
			new ApplicationRole {
				Id = DefaultRoles.AdminRoleId,
				Name = DefaultRoles.Admin,
				NormalizedName = DefaultRoles.Admin.ToUpper(),
				ConcurrencyStamp = DefaultRoles.AdminConcurrencyStamp
			},
			new ApplicationRole {
				Id = DefaultRoles.MemberRoleId,
				Name = DefaultRoles.Member,
				NormalizedName = DefaultRoles.Member.ToUpper(),
				ConcurrencyStamp = DefaultRoles.MemberConcurrencyStamp,
				IsDefault = true
			}
			]
			);
	}
}
