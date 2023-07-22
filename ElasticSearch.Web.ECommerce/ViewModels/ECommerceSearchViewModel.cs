namespace ElasticSearch.Web.ECommerce.ViewModels;

public class ECommerceSearchViewModel
{
	[DisplayName("Category")]
	public string Category { get; set; }

	[DisplayName("Gender")]
	public string Gender { get; set; }

	[DisplayName("Order Date (Start)")]
	[DataType(DataType.Date)]
	public DateTime? OrderDateStart { get; set; }

	[DisplayName("Order Date (End)")]
	[DataType(DataType.Date)]
	public DateTime? OrderDateEnd { get; set; }

	[DisplayName("Customer Full Name")]
	public string CustomerFullName { get; set; }
}
