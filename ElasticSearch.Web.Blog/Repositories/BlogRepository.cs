namespace ElasticSearch.Web.Blogs.Repositories;

public sealed class BlogRepository
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string BLOG_INDEX_NAME = "blog";

    public BlogRepository(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task<Blog> InsertAsync(Models.Blog blog)
    {
        blog.Created = DateTime.Now;

        var result = await _elasticsearchClient.IndexAsync(blog, p => p.Index(BLOG_INDEX_NAME));
        if (!result.IsValidResponse)
        {
            throw new Exception("Adding product failed.");
        }

        blog.Id = result.Id;

        return blog;
    }

    public async Task<List<Blog>> SearchAsync(string searchText)
    {
        var result = await _elasticsearchClient.SearchAsync<Blog>(search => search.Index(BLOG_INDEX_NAME)
            .Size(3)
                .Query(query => query
                    .Bool(b => b
                        .Should(
                            should => should.Match(match => match.Field(field => field.Content).Query(searchText)),
                            should => should.MatchBoolPrefix(matchBoolPrefix => matchBoolPrefix.Field(field => field.Title).Query(searchText))
                                )
                         )
                      )
             );

        foreach (var hit in result.Hits) 
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToList();
    }
}
