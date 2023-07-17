using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ElasticSearch.API.ElasticClientsSearch.Repositories;

public class ECommerceRepository
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string INDEX_NAME = "kibana_sample_data_ecommerce";

    public ECommerceRepository(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task<ImmutableList<ECommerce>> TermQuery(string customerFirstName)
    {
        //1.Yol
        //var result = await _elasticsearchClient.SearchAsync<ECommerce>(
        //    search => search.Index(INDEX_NAME).Query(query => query.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName)))
        //    );

        //2.Yol
        //var result = await _elasticsearchClient.SearchAsync<ECommerce>(
        //    search => search.Index(INDEX_NAME).Query(query => query.Term(t => t.CustomerFirstName.Suffix("keyword"), customerFirstName))
        //    );

        //3.Yol
        var termQuery = new TermQuery("customer_first_name.keyword")
        {
            Value = customerFirstName,
            CaseInsensitive = true
        };

        var result = await _elasticsearchClient.SearchAsync<ECommerce>(
            search => search.Index(INDEX_NAME).Query(termQuery)
            );

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerFirstNameList)
    {
        var customerFirstNameTerms = customerFirstNameList.ConvertAll<FieldValue>(p => p);

        var result = await _elasticsearchClient.SearchAsync<ECommerce>(
            search => search.Index(INDEX_NAME).Size(100)
                                              .Query(query =>
                                                     query.Terms(terms =>
                                                                 terms.Field(fields =>
                                                                             fields.CustomerFirstName.Suffix("keyword")).Terms(new TermsQueryField(customerFirstNameTerms)))));
        //2.Yol
        //var termsQuery = new TermsQuery()
        //{
        //    Field = "customer_first_name.keyword",
        //    Terms = new TermsQueryField(customerFirstNameTerms)
        //};

        //var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME).Query(termsQuery));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> PrefixQuery(string customerFullName)
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
            .Size(50)
                .Query(query => query
                    .Prefix(prefix => prefix
                        .Field(field => field.CustomerFullName.Suffix("keyword"))
                            .Value(customerFullName)))
        );

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> RangeQuery(double fromPrice, double toPrice)
    {
        //gte: Greater Than Or Equals
        //lte: Less Than Or Equals

        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
            .Query(query => query
                .Range(range => range
                    .NumberRange(numberRange => numberRange
                        .Field(field => field.TaxFullTotalPrice)
                            .Gte(fromPrice)
                            .Lte(toPrice))))
        );

        foreach (var hit in result.Hits) //sonuca id değerini eklemek
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }
}
