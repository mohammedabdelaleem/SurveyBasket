using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Authentication;
using SurveyBasket.API.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace SurveyBasket.API.Services;

public class AuthService(
	UserManager<ApplicationUser> userManager,
	AppDbContext context,
	SignInManager<ApplicationUser> signInManager,
	IJWTProvider jWTProvider,
	ILogger<AuthService> logger,
	IEmailSender emailSender,
	IHttpContextAccessor httpContextAccessor
	) : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager = userManager;
	private readonly AppDbContext _context = context;
	private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
	private readonly IJWTProvider _jWTProvider = jWTProvider;
	private readonly ILogger<AuthService> _logger = logger;
	private readonly IEmailSender _emailSender = emailSender;
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
	private readonly int _refreshTokenExpirayDays = 14;

	public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
	{
		// check user by email 
		//var user = await _userManager.FindByEmailAsync(email);

		//if (user == null)
		//return Result.Failure<AuthResponse>(UserErrors.InvalidCredintials);


		if (await _userManager.FindByEmailAsync(email) is not { } user) // instead of the above 2 steps 
			return Result.Failure<AuthResponse>(UserErrors.InvalidCredintials);



		#region check password and Confirmation Email Using _signInManager

		//bool hasPassword = await _userManager.CheckPasswordAsync(user, password);
		//if (!hasPassword)
		//	return Result.Failure<AuthResponse>(UserErrors.InvalidCredintials);
		//if(!user.EmailConfirmed) // this is the check confirmation 1st technique with the userManager , But We Will _signInManager instead 
		//{
		//	return Result.Failure<AuthResponse>( UserErrors.EmailNotConfirmed);
		//}
		#endregion

		// we will check the password using signIn manager
		var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
		if (result.Succeeded)
		{
			return await HandleAuthResponse(user);
		}

		// if we reached here that means
		// 01 - the password is wrong [Invalid Credintials]
		// 02 - Not Confirmed [email]

		return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredintials);
	}


	#region 2 Ways Registeration
	// when you dealing with registeration service we have 2 choices
	// 1 -  Auto-Login : After Register Sent JWT Token To Frontend and Login Without your Interaction 
	// 2 -  Register => Confirm Email => Login 

	// the first way , Learn It For Yourself , We Don't Use It 
	#endregion
	public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken=default)
	{
		// unique email checker 
		var emailIsExists = await _userManager.Users.AnyAsync(u=>u.Email == request.Email, cancellationToken);

		if (emailIsExists)
			return Result.Failure(UserErrors.DuplicatedEmail);


		var user = request.Adapt<ApplicationUser>();
		//user.Email = request.Email;	// Adding newConfiguration Mapping

		var result = await _userManager.CreateAsync(user, request.Password);

		if(result.Succeeded)
		{
			// Generate Code 
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

			_logger.LogInformation("Email Confirmation Code {code}",code);


			// you have 2 choices for backgroud job using hangfire 
			// 01 - Make your Method public 
					//BackgroundJob.Enqueue(() => SendConfirmationEmail(user, code)); // ensure public


			// 02 - Set Job Inside the Method
		

			// Send Email
			await SendConfirmationEmail(user, code);
		

			return Result.Success();
		}


		var error = result.Errors.First();
		return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status409Conflict));
	}

	public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
	{
		if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
			return Result.Failure(UserErrors.InvalidCode);


		if(user.EmailConfirmed)
			return Result.Failure(UserErrors.DuplicatedConfirmation); // send the same request more than 1

		var code = request.Code;
		try
		{ 
			code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
		}
		catch(FormatException)
		{
			return Result.Failure(UserErrors.InvalidCode);
		}


		// now we need to confirm email
		var result = await _userManager.ConfirmEmailAsync(user, code);
		if (result.Succeeded)
	   	   {
			await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
			return Result.Success();
		}


		var error = result.Errors.First();
		return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

	}


	public async Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequest request)
	{
		if(await _userManager.FindByEmailAsync(request.Email) is not { }user)
			return Result.Success(); // here we not telling the truth to the user, may be hacker[confusion]

		if(user.EmailConfirmed)
			return Result.Failure(UserErrors.DuplicatedConfirmation);

		// Generate Code 
		var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

		_logger.LogInformation("Email Confirmation Code {code}", code);


		// Send Email
		await SendConfirmationEmail(user, code);

		return Result.Success();
	}


	public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
	{

		var userId = _jWTProvider.ValidateTokenAndGetUserId(token);

		if (userId == null)
			return Result.Failure<AuthResponse>(TokenErrors.InvalidToken);


		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return Result.Failure<AuthResponse>(UserErrors.UserNotFound);



		var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
		if (userRefreshToken == null)
			return Result.Failure<AuthResponse>(UserErrors.RefreshTokenNotFound);


		// reaching the point means ==> every thing is ok

		userRefreshToken.RevokedOn = DateTime.UtcNow; // use RefreshToken once : buissness requirement

		return await HandleAuthResponse(user);

	}

	public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
	{
		var userId = _jWTProvider.ValidateTokenAndGetUserId(token);
		if (userId == null)
			return Result.Failure<bool>(UserErrors.UserNotFound);


		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return Result.Failure(UserErrors.UserNotFound);


		var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
		if (userRefreshToken == null)
			return Result.Failure(UserErrors.RefreshTokenNotFound);

		// reaching the point means ==> every thing is ok

		userRefreshToken.RevokedOn = DateTime.UtcNow; // use RefreshToken once : buissness requirement
		await _userManager.UpdateAsync(user);

		return Result.Success();
	}

	public async Task<Result> SendResetPasswordCodeAsync(string email)
	{
		if (await _userManager.FindByEmailAsync(email) is not { } user)
			return Result.Success();

		if(!user.EmailConfirmed)
			return Result.Failure(UserErrors.EmailNotConfirmed);


		// find a user with the incomming email ==>
		 /*
		   Generate a token
				Send it via email(or link)
				When the user clicks the link, they submit the token along with their new password
		 */

		// 01 - Generate Code 
		var code = await _userManager.GeneratePasswordResetTokenAsync(user);
		code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

		_logger.LogInformation("Reset Code :{code}", code);


		// 02 - Send Email
		await SendResetPasswordEmail(user, code);

		return Result.Success();
	}

	public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		if(user is null || !user.EmailConfirmed)
			return Result.Failure(UserErrors.InvalidCode);

		IdentityResult result;

		try
		{
			var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
			result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
		}
		catch (FormatException)
		{
			result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
		}

		if(result.Succeeded)
			return Result.Success();


		var error = result.Errors.First();
		return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));	
	}	




	private async Task<Result<AuthResponse>> HandleAuthResponse(ApplicationUser user, CancellationToken cancellationToken=default)
	{


		var (UserRoles, permissions) = await GetUserRolesAndPermissions(user, cancellationToken);

		// Generate JWT Token 
		var (newToken, expiresIn) = _jWTProvider.GenerateToken(user,UserRoles, permissions);

		// generate refresh token
		var newRefreshToken = GenerateRefreshToken();
		var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirayDays);


		// we need to save this new refresh token at the DB 
		// we have the user and the refresh tokens are owned for user

		user.RefreshTokens.Add(new RefreshToken
		{
			Token = newRefreshToken,
			ExpiresOn = refreshTokenExpiration
		});

		await _userManager.UpdateAsync(user);

		// Return Response
		var response= new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName,
			newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

		return Result.Success(response);
	}

	private async Task<(IEnumerable<string> UserRoles ,IEnumerable<string> permissions )> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken =default)
	{
		var userRoles = await _userManager.GetRolesAsync(user);

		#region permissins using chain linq methods

		//var permissions = await _context.Roles
		//		.Join(_context.RoleClaims,
		//			  role=>role.Id,
		//			  roleClaim => roleClaim.RoleId,
		//			  (role,roleClaim)=> new {role , roleClaim })
		//		.Where(x=>userRoles.Contains(x.role.Name!))
		//		.Select(x=>x.roleClaim.ClaimValue!)
		//		.Distinct()
		//		.ToListAsync(cancellationToken);


		#endregion

		// query syntax is a good way with join
		var permissions = await (
				from r in _context.Roles
				join rc in _context.RoleClaims 
				on r.Id equals rc.RoleId
				where userRoles.Contains(r.Name!)
				select rc.ClaimValue!)
				.Distinct()
				.ToListAsync(cancellationToken);

		return (userRoles, permissions!);
	}
		 
	private string GenerateRefreshToken()
	{
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}
	private async Task SendConfirmationEmail(ApplicationUser user , string code)
	{
		// may be requests come from specific url [development , production, testing, etc] and you can add them add appsettings 
		// but here we choose the difficult way

		// dynaic way to know the request url
		var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

		var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
			new Dictionary<string, string> {
				{"{{name}}", $"{user.FirstName} {user.LastName}"},
				{"{{action_url}}", $"{origin}/auth/EmailConfirmation?userId={user.Id}&code={code}" },
				
				// the path after origin :it's a frontend responsibility => must telling you what is the path after click on the	correct path end user should go on , can send it at headers	  
				// 2 values which front end need to resend them to me again
			}
			) ;

		//await _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket : Email Confirmation ", emailBody);

		BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket : Email Confirmation ", emailBody));
		await Task.CompletedTask;


	}

	private async Task SendResetPasswordEmail(ApplicationUser user, string code)
	{
		// may be requests come from specific url [development , production, testing, etc] and you can add them add appsettings 
		// but here we choose the difficult way

		// dynaic way to know the request url
		var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

		var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
			new Dictionary<string, string> {
				{"{{name}}", $"{user.FirstName} {user.LastName}"},
				{"{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" },
				
				// the path after origin :it's a frontend responsibility => must telling you what is the path after click on the	correct path end user should go on , can send it at headers	  
				// 2 values which front end need to resend them to me again
			}
			);

		//await _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket : Email Confirmation ", emailBody);

		BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket : Change Password ", emailBody));
		await Task.CompletedTask;


	}


}
