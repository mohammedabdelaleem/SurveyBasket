
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.API.Authentication;

public class JWTProvider : IJWTProvider
{
	public (string token, int expiresIn) GenerateToken(ApplicationUser user)
	{

		// Claims Which Needed From Frontend , Card info 
		Claim[] claims = [
			
			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
			new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
			new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			];


		// Key for En/Decoding
		var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bxCTTfkq3heROwz2wvI5pGo8QmWYkOQB"));
	
		var signInCredintials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

		var expiresIn = 30;


		var token = new JwtSecurityToken(
			issuer: "SurveyBasket",   // who exports the token
			audience: "SurveyBasket users", // who are the users for this token
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(expiresIn),
			signingCredentials: signInCredintials
			);

		return (token:new JwtSecurityTokenHandler().WriteToken(token) , expiresIn: expiresIn * 60);
	}
}
