namespace ElasticSearch.API.ElasticClientsSearch.DTOs
{
    public record ProductFeatureDto
    {
        public int Width { get; init; }
        public int Height { get; init; }
        public string Color { get; init; }
    }
}
