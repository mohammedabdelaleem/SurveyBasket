namespace SurveyBasket.API.Entities;


// RefreshTokens will be assigned for the specific user
// like its phone numbers 
// also you didn't need to add the entity to your AppDBContext

// even if you didn't add the Id [PK] after migration , it will add automatically
// it will add also the forign key for the owner
// ======> The PK For The Table Will Be Composite Between Both	
[Owned]
public sealed class RefreshToken
{
	public string Token { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime ExpiresOn { get; set; }

	public DateTime? RevokedOn { get; set; }
	public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
	public bool IsActive => RevokedOn is null && !IsExpired;

}
