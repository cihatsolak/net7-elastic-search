namespace ElasticSearch.Web.ECommerce.Services;

public class ECommerceService
{
	private readonly ECommerceRepository _repository;

	public ECommerceService(ECommerceRepository repository)
	{
		_repository = repository;
	}

	public async Task<(List<ECommerceViewModel>, long, long)> SearchAsync(ECommerceSearchViewModel searchViewModel, int pageIndex, int pageSize)
	{
		var (ecommerceList, totalRecords) = await _repository.SearchAsync(searchViewModel, pageIndex, pageSize);

		int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

		var eCommerceListViewModel = ecommerceList.Select(ecommerce => new ECommerceViewModel
		{
			Category = string.Join(",", ecommerce.Category),
			CustomerFirstName = ecommerce.CustomerFirstName,
			CustomerLastName = ecommerce.CustomerLastName,
			CustomerFullName = ecommerce.CustomerFullName,
			OrderDate = ecommerce.OrderDate.ToShortDateString(),
			Gender = ecommerce.Gender.ToLower(),
			Id = ecommerce.Id,
			OrderId = ecommerce.OrderId,
			TaxFullTotalPrice = ecommerce.TaxFullTotalPrice
		}).ToList();

		return (eCommerceListViewModel, totalRecords, totalPages);
	}
}
