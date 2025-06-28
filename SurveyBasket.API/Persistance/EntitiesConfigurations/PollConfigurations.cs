
namespace SurveyBasket.API.Persistance.EntitiesConfigurations;

public class PollConfigurations : IEntityTypeConfiguration<Poll>
{
	public void Configure(EntityTypeBuilder<Poll> builder)
	{
		// uniquness using index
		builder.HasIndex(x => x.Title).IsUnique();

		builder.Property(x => x.Title).HasMaxLength(100);
		builder.Property(x => x.Summary).HasMaxLength(1500);

	}
}
