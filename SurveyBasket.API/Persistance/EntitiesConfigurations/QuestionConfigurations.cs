
namespace SurveyBasket.API.Persistance.EntitiesConfigurations;

public class QuestionConfigurations : IEntityTypeConfiguration<Question>
{
	public void Configure(EntityTypeBuilder<Question> builder)
	{
		// uniquness using index
		builder.HasIndex(x => new { x.PollId, x.Content }).IsUnique();

		builder.Property(x => x.Content).HasMaxLength(1000);

	}
}
