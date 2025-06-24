namespace SurveyBasket.API.Contracts.Users;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
	public CreateUserRequestValidator()
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

		RuleFor(x => x.Password)
			.NotEmpty()
			.Matches(RegexPatterns.Password)
			.WithMessage("Password Should Be At Least 8 Digits And Should Contains Lowercase , Uppercase And NonAlphanumeric");

		RuleFor(x => x.Roles)
			.NotEmpty()
			.NotNull();

		RuleFor(x => x.Roles)
		.Must(r=>r.Distinct().Count() == r.Count)
		.WithMessage("Roles Must Be Distinct, No Duplicates.")
		.When(x=>x.Roles != null);

	}
}
