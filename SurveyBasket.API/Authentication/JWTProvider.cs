
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.API.Authentication;

public class JWTProvider(IOptions<JWTOptions> jwtOptions) : IJWTProvider
{
	private readonly JWTOptions _jwtOptions = jwtOptions.Value;
	

	public (string token, int expiresIn) GenerateToken(ApplicationUser user)
	{
		//var jwtSettings = 
		// Claims Which Needed From Frontend , Card info 
		Claim[] claims = [
			
			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
			new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
			new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			];


		// Key for En/Decoding
		var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
	
		var signInCredintials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

		var expiresIn = _jwtOptions.ExpiryMinutes;


		var token = new JwtSecurityToken(
			issuer: _jwtOptions.Issuer,   // who exports the token
			audience: _jwtOptions.Audience, // who are the users for this token
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(expiresIn),
			signingCredentials: signInCredintials
			);

		return (token:new JwtSecurityTokenHandler().WriteToken(token) , expiresIn: expiresIn * 60);
	}
}
