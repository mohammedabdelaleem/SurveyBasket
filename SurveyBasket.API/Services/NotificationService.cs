using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SurveyBasket.API.Helpers;

namespace SurveyBasket.API.Services;

public class NotificationService(
	AppDbContext context,
	UserManager<ApplicationUser> userManager,
	HttpContextAccessor httpContextAccessor,
	IEmailSender emailSender) : INotificationService
{
	private readonly AppDbContext _context = context;
	private readonly UserManager<ApplicationUser> _userManager = userManager;
	private readonly HttpContextAccessor _httpContextAccessor = httpContextAccessor;
	private readonly IEmailSender _emailSender = emailSender;

	public async Task SendNewPollsNotification(int? pollId = null)
	{
	IEnumerable<Poll> polls = [];

		if(pollId.HasValue)
		{
			var poll = await _context.Polls.SingleOrDefaultAsync(p=> p.Id == pollId&& p.IsPublished );
			polls = [poll!];
		}
		else
		{
			polls = await _context.Polls
				.Where(p=>
					p.IsPublished &&
					p.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
				.AsNoTracking()
				.ToListAsync();
		}

		// TODO: select members only
		var users = await _userManager.Users.ToListAsync();


		// foreach availabel poll we have 
		// send notification for this poll to all users in the system 
		// hey users , this poll is availabe today for voting 
		

		var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

		foreach (var poll in polls)
		{
			foreach (var user in users)
			{
				// prepare placeholders
				var placeholders = new Dictionary<string, string>()
				{
					{"{{name}}", user.FirstName },
					{"{{pollTill}}", poll.Title },
					{"{{endDate}}",poll.EndsAt.ToString("dd/MM/yyyy") },
					{"{{url}}",$"{origin}/polls/start/{poll.Id}"  }
				};

				// generate body
				var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholders);

				// send email
				await _emailSender.SendEmailAsync(user.Email! , $"🔦 Survey Basket : New Poll - {poll.Title}", body);
			}
		}
		
	}
}
