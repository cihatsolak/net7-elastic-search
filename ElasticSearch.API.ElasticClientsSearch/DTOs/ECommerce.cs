namespace ElasticSearch.API.ElasticClientsSearch.DTOs.EcommerceModel
{
    public class ECommerce
    {
        [JsonPropertyName("_id")] //Nest kütüphanesinde [PropertyName("")] attribütünü kullanıyoruz
        public string Id { get; set; }

        [JsonPropertyName("customer_first_name")]
        public string CustomerFirstName { get; set; }

        [JsonPropertyName("customer_last_name")]
        public string CustomerLastName { get; set; }

        [JsonPropertyName("customer_full_name")]
        public string CustomerFullName { get; set; }

        [JsonPropertyName("category")]
        public string[] Category { get; set; }

        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("order_date")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("products")]
        public EcommerceProduct[] Products { get; set; }
    }

    public class EcommerceProduct
    {
        [JsonPropertyName("product_id")]
        public long ProductId { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
    }
}
