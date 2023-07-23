namespace ElasticSearch.Web.ECommerce.ViewModels;

public class SearchPageViewModel
{
	public long TotalCount { get; set; }
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public long PageLinkCount { get; set; }
	public List<ECommerceViewModel> List { get; set; }
	public ECommerceSearchViewModel SearchViewModel { get; set; }

	public string CreatePageUrl(HttpRequest request, int page, int pageSize)
	{
		string currentUrl = new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}").AbsoluteUri;

		if (currentUrl.Contains("Page", StringComparison.OrdinalIgnoreCase))
		{
			currentUrl = currentUrl.Replace($"Page={Page}", $"Page={page}", StringComparison.OrdinalIgnoreCase);

			currentUrl = currentUrl.Replace($"PageSize={Page}", $"PageSize={page}", StringComparison.OrdinalIgnoreCase);
		}
		else
		{
			currentUrl = $"{currentUrl}?Page={page}&PageSize={pageSize}";
		}

		return currentUrl;
	}
}