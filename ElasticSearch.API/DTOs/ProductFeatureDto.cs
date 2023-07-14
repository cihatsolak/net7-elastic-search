namespace ElasticSearch.API.DTOs
{
    public record ProductFeatureDto
    {
        public int Width { get; init; }
        public int Height { get; init; }
        public EColor Color { get; init; }
    }
}
