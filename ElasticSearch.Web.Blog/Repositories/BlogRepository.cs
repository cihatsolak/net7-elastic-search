namespace ElasticSearch.Web.Blog.Repositories;

public sealed class BlogRepository
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string INDEX_NAME = "blog";

    public BlogRepository(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }
}
