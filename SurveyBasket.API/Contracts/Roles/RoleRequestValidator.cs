namespace SurveyBasket.API.Contracts.Roles;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
	public RoleRequestValidator()
	{
		RuleFor(x=>x.Name)
			.NotEmpty()
			.Length(3,256);

		RuleFor(x => x.Permissions)
			.NotNull()
			.NotEmpty();

		RuleFor(x => x.Permissions)
				.Must(x => x.Distinct().Count() == x.Count)
				.WithMessage("Can't Add Duplicate Permissions For The Same Role")
				.When(x => x.Permissions is not null);
	}
}
