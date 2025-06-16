
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using SurveyBasket.API.Settings;

namespace SurveyBasket.API.Services;


/*
	It implements the IEmailSender interface from ASP.NET Identity,
	which is used when sending account confirmation emails, password resets, notifications, etc.
 */
public class EmailService(IOptions<MailSettings> mailSettings ,ILogger<EmailService> logger) : IEmailSender
{
	private readonly MailSettings _mailSettings = mailSettings.Value;
	private readonly ILogger<EmailService> _logger = logger;

	public async Task SendEmailAsync(string email, string subject, string htmlMessage)
	{
		var emailMessage = new MimeMessage 
		{
			Sender = MailboxAddress.Parse(_mailSettings.Mail),
			Subject = subject,
		};

		emailMessage.To.Add(MailboxAddress.Parse(email)); // To - CC - BCC  ??

		/*
		 To  : Main recipient - Visible to Others - People who should act on the email [[manager]] -- Can't see BCC 
		 CC  : Inform only -  Visible to Others - People who should be aware [[hr]] -- Can't see BCC
		 BCC : Purpose:Hidden copyv , Not Visible to Others - People you want to inform silently [[CEO]] -- Sees all (but no one sees the Him)s
		 */

		var builder = new BodyBuilder
		{
			HtmlBody = htmlMessage
		};

		emailMessage.Body = builder.ToMessageBody();

		using var smtp = new SmtpClient(); //Simple Mail Transfer Protocol.

		_logger.LogInformation("sending email to : {email}", email);

		smtp.Connect(_mailSettings.Host, _mailSettings.Port,SecureSocketOptions.StartTls);
		smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
		await smtp.SendAsync(emailMessage);

		smtp.Disconnect(true);

	}
}
