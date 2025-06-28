namespace SurveyBasket.API.Contracts.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();
		//.MustAsync; // we can check if any email at db has the same incomming email at db to apply uniquness 


		RuleFor(x => x.Password)
			.NotEmpty()
			.Matches(RegexPatterns.Password)
			.WithMessage("Password Should Be At Least 8 Digits And Should Contains Lowercase , Uppercase And NonAlphanumeric");

		RuleFor(x => x.FirstName)
			.NotEmpty()
			.Length(3, 100); // look at DB to prevent the trancation 

		RuleFor(x => x.LastName)
			.NotEmpty()
			.Length(3, 100);

	}
}
