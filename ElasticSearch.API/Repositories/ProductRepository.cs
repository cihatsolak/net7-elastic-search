﻿using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _elasticClient;
    private const string PRODUCT_INDEX_NAME = "products";

    public ProductRepository(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
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
        var result = await _elasticClient.IndexAsync(product, p => p.Index(PRODUCT_INDEX_NAME).Id(id));
        if (!result.IsValid)
        {
            throw new Exception("Adding product failed.");
        }

        product.Id = result.Id;

        return product;
    }

    public async Task<ImmutableList<Product>> GetAllAsync()
    {
        var result = await _elasticClient.SearchAsync<Product>(search => search.Index(PRODUCT_INDEX_NAME).Query(query => query.MatchAll()));
        if (!result.IsValid)
        {
            return (ImmutableList<Product>)Enumerable.Empty<Product>();
        }

        foreach (var hit in result.Hits) //ürün id'lerini almak
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        var response = await _elasticClient.GetAsync<Product>(id, x => x.Index(PRODUCT_INDEX_NAME));
        if (!response.IsValid)
        {
            return default;
        }

        response.Source.Id = response.Id;

        return response.Source;
    }
}
