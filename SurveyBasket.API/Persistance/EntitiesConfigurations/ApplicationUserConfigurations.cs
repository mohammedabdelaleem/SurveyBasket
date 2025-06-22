
namespace SurveyBasket.API.Persistance.EntitiesConfigurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.Property(x => x.FirstName)
			.HasMaxLength(100);
		builder.Property(x => x.LastName)
			.HasMaxLength(100);



		builder
			.OwnsMany(x => x.RefreshTokens)
			.ToTable("RefreshTokens") // instead of RefreshToken
			.WithOwner()
			.HasForeignKey("UserId"); // // instead of ApplicaionUserId



		builder.HasData(new ApplicationUser
		{
			Id = DefaultUsers.AdminId,
			FirstName = "Survey Basket",
			LastName = "Admin",
			Email = DefaultUsers.AdminEmail,
			NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
			UserName = DefaultUsers.AdminEmail,
			NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
			ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
			SecurityStamp = DefaultUsers.AdminSecurityStamp,
			EmailConfirmed = true, // don't forget this , We don't need default admin to sign in
			PasswordHash =DefaultUsers.AdminPasswordHash 
		});
	}
}
