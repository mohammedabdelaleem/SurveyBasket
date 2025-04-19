namespace SurveyBasket.API.Entities;

public class Poll : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
	public bool  IsPublished { get; set; }
    public DateOnly StartsAt { get; set; }=DateOnly.FromDateTime(DateTime.Now);
	public DateOnly EndsAt { get; set; }

}
