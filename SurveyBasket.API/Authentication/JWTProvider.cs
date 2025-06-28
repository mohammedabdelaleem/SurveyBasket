
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SurveyBasket.API.Authentication;

public class JWTProvider(IOptions<JWTOptions> jwtOptions) : IJWTProvider
{
	private readonly JWTOptions _jwtOptions = jwtOptions.Value;
	

	public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles , IEnumerable<string> permissions)
	{

		// Claims Which Needed From Frontend , Card info 
		Claim[] claims = [
			
			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
			new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
			new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()), // prefer to adding with claims ,
			new Claim(nameof(roles) , JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray),
			new Claim(nameof(permissions) , JsonSerializer.Serialize(permissions), JsonClaimValueTypes.JsonArray)
			];

		//new Claim(nameof(roles), string.Join(",", roles)),
		//	new Claim(nameof(permissions), string.Join(",", permissions)),

		// Key for En/Decoding
		var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
	
		// Encryption Algorithm
		var signInCredintials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

		var expiresIn = _jwtOptions.ExpiryMinutes;
		
		// information that added with token	
		var token = new JwtSecurityToken(
			issuer: _jwtOptions.Issuer,   // who exports the token : your auth server [iss and aud are excellent for microservices] 
			audience: _jwtOptions.Audience, // who are the users for this token : your api 
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(expiresIn),
			signingCredentials: signInCredintials
			);

		return (token:new JwtSecurityTokenHandler().WriteToken(token) , expiresIn: expiresIn * 60);
	}

	// validate and retutn the user id if token is ok 
	public string? ValidateTokenAndGetUserId(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		// Same Key : which we are used for Encryption , now we need to use it with Decryption
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
			//now we have the access (jwt) token with its claims
			// we need to return the userId , to use it later and credintial the current user

			return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
		}
		catch 
		{ 
		return null;
		}

	}
}
