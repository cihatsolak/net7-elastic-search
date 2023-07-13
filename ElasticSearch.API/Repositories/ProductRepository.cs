namespace ElasticSearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _elasticClient;

    public ProductRepository(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<Product> InsertAsync(Product product)
    {
        product.Created = DateTime.Now;
        product.Updated = null;

        //var response = await _elasticClient.IndexAsync(data, Tablo ismi);
        var result = await _elasticClient.IndexAsync(product, p => p.Index("Products"));
        if (!result.IsValid)
        {
            throw new Exception("Adding product failed.");
        }

        product.Id = result.Id;
        
        return product;
    }
}
