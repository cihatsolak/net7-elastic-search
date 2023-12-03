namespace ElasticSearch.Web.ECommerce.ViewModels;

public class ECommerceViewModel
{
    public string Id { get; set; }

    public string CustomerFirstName { get; set; }

    public string CustomerLastName { get; set; }

    public string CustomerFullName { get; set; }

    public string Gender { get; set; }

    public double TaxFullTotalPrice { get; set; }

    public string Category { get; set; }

    public int OrderId { get; set; }

    public string OrderDate { get; set; }
}
