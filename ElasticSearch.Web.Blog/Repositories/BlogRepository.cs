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

        var result = await _elasticsearchClient.IndexAsync(blog, p => p.Index("products"));
        if (!result.IsValidResponse)
        {
            throw new Exception("Adding product failed.");
        }

        blog.Id = result.Id;

        return blog;
    }
}
