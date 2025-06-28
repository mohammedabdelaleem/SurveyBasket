
namespace SurveyBasket.API.Persistance.EntitiesConfigurations;

public class AnswerConfigurations : IEntityTypeConfiguration<Answer>
{
	public void Configure(EntityTypeBuilder<Answer> builder)
	{
		// uniquness using index
		builder.HasIndex(x => new { x.QuestionId, x.Content }).IsUnique();

		builder.Property(x => x.Content).HasMaxLength(1000);

	}
}
