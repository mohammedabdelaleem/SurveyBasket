
namespace SurveyBasket.API.Persistance.EntitiesConfigurations;

public class VoteConfigurations : IEntityTypeConfiguration<Vote>
{
	public void Configure(EntityTypeBuilder<Vote> builder)
	{
		// uniquness using index on the table itself
	builder.HasIndex(x=>new { x.PollId , x.UserId}).IsUnique();

	}
}
