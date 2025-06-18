namespace SurveyBasket.API.Services;

public interface INotificationService
{
	// Sending Notifications To Polls At 00:00 , you create new poll or make someone is published at 03:00 at this case the admin or the system needs to send to this poll specifically
	Task SendNewPollsNotification(int? pollId = null);
}
