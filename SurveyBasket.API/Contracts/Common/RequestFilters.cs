namespace SurveyBasket.API.Contracts.Common;

public record RequestFilters
{
	public int PageNumber { get; init; } = 1;
	public int PageSize { get; init; } = 10;
	public string? SearchValue { get; init; }
	public string? SortColumn { get; init; } = "Id";
	public string? SortDirection { get; init; } = "ASC";

}
