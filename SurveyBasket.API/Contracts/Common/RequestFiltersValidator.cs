namespace SurveyBasket.API.Contracts.Common;

public class RequestFiltersValidator : AbstractValidator<RequestFilters>
{
	public RequestFiltersValidator()
	{
		// applying default values if missing 

		RuleFor(x => x.PageNumber)
			.GreaterThanOrEqualTo(1)
			.WithMessage("PageNumber must be greater than or equal to 1.");

		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 15)
			.WithMessage("PageSize must be between 1 and 15.");
	}
}
