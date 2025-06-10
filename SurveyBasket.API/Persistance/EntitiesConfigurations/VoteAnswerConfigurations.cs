
namespace SurveyBasket.API.Persistance.EntitiesConfigurations;

public class VoteAnswerConfigurations : IEntityTypeConfiguration<VoteAnswer>
{
	public void Configure(EntityTypeBuilder<VoteAnswer> builder)
	{
		builder.HasIndex(x => new { x.VoteId, x.QuestionId }).IsUnique();
	}
}
