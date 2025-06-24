namespace SurveyBasket.API.Abstractions;

public class PaginationList<T>(List<T> items, int pageNumber, int pageSize, int count)
{
	public List<T> Items { get; private set; } = items;
	public int PageNumber { get; private set; } = pageNumber;
	public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize); 
	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;

	public static async Task<PaginationList<T>> CreateAsync(IQueryable<T> source , int pageNumber , int pageSize, CancellationToken cancellationToken=default)
	{
		int count = await source.CountAsync(cancellationToken);
		var items = await source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync(cancellationToken);

		return new PaginationList<T>(items, pageNumber, pageSize, count);
	}

}
