namespace SurveyBasket.API.Contracts.Auth;

public class ResendEmailConfirmationRequestValidator : AbstractValidator<ResendEmailConfirmationRequest>
{
	public ResendEmailConfirmationRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();
	}
}
