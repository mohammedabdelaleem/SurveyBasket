namespace SurveyBasket.API.Entities;

public sealed class ApplicationUser : IdentityUser
{
	public string FirstName { get; set; }
	public string LastName { get; set; }


	// owned entity , we don't need to add its table at AppDbContext
	// it will generate automatically
	public List<RefreshToken> RefreshTokens { get; set; } = [];

}
