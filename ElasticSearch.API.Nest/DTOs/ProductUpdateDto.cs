namespace ElasticSearch.API.Nest.DTOs;

public record ProductUpdateDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public float Price { get; init; }
    public int Stock { get; init; }
    public ProductFeatureDto Feature { get; init; }
}
