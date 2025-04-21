
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

	public string? ValidateToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		// Same Key 
		// Key for En/Decoding
		var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				IssuerSigningKey = symmetricSecurityKey,
				ValidateIssuerSigningKey = true,
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero, // once the expire time occures , it calculate it as expire don't still 5m
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;

			return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
		}
		catch 
		{ 
		return null;
		}



	}
}
