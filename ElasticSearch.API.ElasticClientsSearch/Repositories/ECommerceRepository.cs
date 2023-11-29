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
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(
            search => search.Index(INDEX_NAME)
                                .Query(query => query
                                    .Term(term => term
                                        .Field(field => field.CustomerFirstName.Suffix("keyword"))
                                            .Value(customerFirstName)
                                                .CaseInsensitive())));

        //2.Yol
        //var result = await _elasticsearchClient.SearchAsync<ECommerce>(
        //    search => search.Index(INDEX_NAME).Query(query => query.Term(t => t.CustomerFirstName.Suffix("keyword"), customerFirstName))
        //    );

        //3.Yol
        //var termQuery = new TermQuery("customer_first_name.keyword")
        //{
        //    Value = customerFirstName,
        //    CaseInsensitive = true
        //};

        //var result = await _elasticsearchClient.SearchAsync<ECommerce>(
        //    search => search.Index(INDEX_NAME).Query(termQuery)
        //    );

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerFirstNameList)
    {
        var customerFirstNameTerms = customerFirstNameList.ConvertAll<FieldValue>(property => property);

        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                                .Size(100)
                                  .Query(query => query
                                      .Terms(terms => terms
                                          .Field(field => field.CustomerFirstName.Suffix("keyword"))
                                              .Terms(new TermsQueryField(customerFirstNameTerms)))));
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

    public async Task<ImmutableList<ECommerce>> PrefixQuery(string customerFullName) //StartWith
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                                .Size(50)
                                    .Query(query => query
                                        .Prefix(prefix => prefix
                                            .Field(field => field.CustomerFullName.Suffix("keyword"))
                                                .Value(customerFullName)
                                                    .CaseInsensitive(true))));

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
                                                    .Lte(toPrice)))));

        //var result2 = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
        //                        .Size(10)
        //                            .Query(query => query
        //                                .Range(range => range
        //                                    .DateRange(dateRange => dateRange
        //                                        .Field(field => field.OrderDate)
        //                                            .Gt(DateTime.Now)
        //                                               .Lt(DateTime.Now)))));

        foreach (var hit in result.Hits) //sonuca id değerini eklemek
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchAllQuery()
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                            .Size(100)
                                .Query(query => query
                                     .MatchAll()));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> PaginationQuery(int pageIndex, int pageSize)
    {
        //page=1 pageSize=10 => 1-10
        //page=2 pageSize=10 => 11-20
        //page=3 pageSize=10 => 21-30

        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                        .From(pageSize * (pageIndex - 1))
                            .Size(pageSize)
                                .Query(query => query
                                     .MatchAll()));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> WildCardQuery(string customerFullName) //LIKE %Test%
    {
        //customerFullName -> Cih?t gibi request'den gelecek.

        //Lam*    --> lam ile başlayan sonu ne olursa olsun
        //Lam*rt  --> lam ile başlayıp rt ile biten ve arada hangi harfler olursa olsun
        //Lambe?t --> Lambe ile başlayan, 7 karakterden oluşan, sonu t harfi ile biten  ve soru işareti yerine herhangi bir harf olabilecek

        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                            .Query(query => query
                                 .Wildcard(wilcard => wilcard
                                    .Field(field => field.CustomerFullName.Suffix("keyword"))
                                        .Value(customerFullName)
                                           .CaseInsensitive())));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> FuzzyAndOrderQuery(string customerName) //LIKE %Test%
    {
        //.Fuzziness(new Fuzziness(1)) --> 1 harf hatasını tolere et

        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                .Query(query => query
                     .Fuzzy(wilcard => wilcard
                        .Field(field => field.CustomerFirstName.Suffix("keyword"))
                            .Value(customerName)
                                .Fuzziness(new Fuzziness(1))))
                                    .Sort(sort => sort
                                            .Field(field => field.TaxFullTotalPrice, new FieldSort() { Order = SortOrder.Desc }))
        );

        foreach (var hit in result.Hits) //sonuca id değerini eklemek
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchQueryFullText(string categoryName)
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                .Query(query => query
                     .Match(match => match
                        .Field(field => field.Category)
                            .Query(categoryName))));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MultiMatchQueryFullText(string name)
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                .Query(query => query
                     .MultiMatch(multiMatch => multiMatch
                        .Fields(new Field("customer_first_name").And(new Field("customer_last_name")).And(new Field("customer_full_name")))
                            .Query(name))));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchBooleanPrefixFullTextQuery(string customerFullName)
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                .Query(query => query
                     .MatchBoolPrefix(matchBoolPrefix => matchBoolPrefix
                        .Field(field => field.CustomerFullName)
                            .Query(customerFullName))));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchPhraseFullTextQuery(string customerFullName)
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                .Query(query => query
                     .MatchPhrase(matchPhrase => matchPhrase
                        .Field(field => field.CustomerFullName)
                            .Query(customerFullName))));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> CompoundQueryExampleOneQuery(string cityName, double taxFullTotalPrice, string categoryName, string manufacturer)
    {
        var result = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                    .Size(500)
                        .Query(query => query
                             .Bool(b => b
                                .Must(must => must
                                    .Term(term => term
                                        .Field("geoip.city_name")
                                            .Value(cityName)))
                                .MustNot(mustNot => mustNot
                                    .Range(range => range
                                        .NumberRange(numberRange => numberRange
                                            .Field(field => field.TaxFullTotalPrice)
                                                .Lte(taxFullTotalPrice))))
                                .Should(should => should
                                    .Term(term => term
                                        .Field(field => field.Category.Suffix("keyword"))
                                            .Value(categoryName)))
                                .Filter(filter => filter
                                    .Term(term => term
                                        .Field("manufacturer.keyword")
                                            .Value(manufacturer)))
                                )));

        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> CompoundQueryExampleTwoQuery(string customerFullName)
    {
        var result1 = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                    .Size(500)
                        .Query(query => query
                             .Bool(b => b
                                .Should(should => should
                                    .Match(term => term
                                        .Field(field => field)
                                            .Query(customerFullName))
                                    .Prefix(prefix => prefix
                                        .Field(field => field.CustomerFullName.Suffix("keyword"))
                                            .Value(customerFullName))
                                        )
                                )));


        var result2 = await _elasticsearchClient.SearchAsync<ECommerce>(search => search.Index(INDEX_NAME)
                  .Size(500)
                      .Query(query => query
                        .MatchPhrasePrefix(matchPhrasePrefix => matchPhrasePrefix
                            .Field(field => field.CustomerFullName)
                                .Query(customerFullName))));

        return result1.Documents.ToImmutableList();
    }
}
