namespace ElasticSearch.API.ElasticClientsSearch.DTOs
{
    public record ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public ProductFeatureDto Feature { get; set; }
    }
}
