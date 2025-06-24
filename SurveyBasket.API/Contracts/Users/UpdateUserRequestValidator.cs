namespace SurveyBasket.API.Contracts.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
	public UpdateUserRequestValidator()
	{

		RuleFor(x => x.FirstName)
				.NotEmpty()
				.Length(3, 100);

		RuleFor(x => x.LastName)
				.NotEmpty()
				.Length(3, 100);

		RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();


		RuleFor(x => x.Roles)
				.NotEmpty()
				.NotNull();

		RuleFor(x => x.Roles)
			.Must(r => r.Distinct().Count() == r.Count)
			.WithMessage("Roles Must Be Distinct, No Duplicates.")
			.When(x => x.Roles != null);

	}
}

