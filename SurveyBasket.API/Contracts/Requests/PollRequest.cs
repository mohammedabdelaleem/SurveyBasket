namespace SurveyBasket.API.Contracts.Requests;

public class PollRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
