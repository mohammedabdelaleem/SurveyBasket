namespace SurveyBasket.API.Entities;

public class AuditableEntity
{

	// we need to know who created and updated this Poll record
	public string CreatedById { get; set; } = string.Empty;
	public ApplicationUser CreatedBy { get; set; } = default!;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


	public string? UpdatedById { get; set; }				
	public ApplicationUser? UpdatedBy { get; set; }
	public DateTime? UpdatedAt { get; set; }
}
