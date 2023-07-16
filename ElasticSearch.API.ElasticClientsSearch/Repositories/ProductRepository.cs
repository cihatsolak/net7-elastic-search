namespace ElasticSearch.API.ElasticClientsSearch.Repositories;

public class ProductRepository
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string PRODUCT_INDEX_NAME = "products";

    public ProductRepository(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task<Product> InsertAsync(Product product)
    {
        product.Created = DateTime.Now;
        product.Updated = null;

        //var response = await _elasticClient.IndexAsync(data, Tablo ismi);
        //var result = await _elasticClient.IndexAsync(product, p => p.Index("products"));
        //if (!result.IsValid)
        //{
        //    throw new Exception("Adding product failed.");
        //}

        //id belirleme
        string id = Guid.NewGuid().ToString();
        var response = await _elasticsearchClient.IndexAsync(product, p => p.Index(PRODUCT_INDEX_NAME).Id(id));
        if (!response.IsValidResponse)
        {
            throw new Exception("Adding product failed.");
        }

        product.Id = response.Id;

        return product;
    }

    public async Task<ImmutableList<Product>> GetAllAsync()
    {
        var response = await _elasticsearchClient.SearchAsync<Product>(search => search.Index(PRODUCT_INDEX_NAME).Query(query => query.MatchAll()));
        if (!response.IsValidResponse)
        {
            return (ImmutableList<Product>)Enumerable.Empty<Product>();
        }

        foreach (var hit in response.Hits) //ürün id'lerini almak
        {
            hit.Source.Id = hit.Id;
        }

        return response.Documents.ToImmutableList();
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        var response = await _elasticsearchClient.GetAsync<Product>(id, x => x.Index(PRODUCT_INDEX_NAME));
        if (!response.IsValidResponse)
        {
            return default;
        }

        response.Source.Id = response.Id;

        return response.Source;
    }

    public async Task<bool> UpdateAsync(ProductUpdateDto productUpdateDto)
    {
        var updateResponse = await _elasticsearchClient.UpdateAsync<Product, ProductUpdateDto>(PRODUCT_INDEX_NAME, productUpdateDto.Id, x => x.Doc(productUpdateDto));
        return updateResponse.IsValidResponse;
    }

    public async Task<DeleteResponse> DeleteAsync(string id)
    {
        var deleteResponse = await _elasticsearchClient.DeleteAsync<Product>(id, x => x.Index(PRODUCT_INDEX_NAME));
        return deleteResponse;
    }
}
