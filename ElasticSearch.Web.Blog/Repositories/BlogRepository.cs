using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Clients.Elasticsearch;

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
		List<Action<QueryDescriptor<Blog>>> listQuery = new();

		Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll();

		Action<QueryDescriptor<Blog>> matchContent = (q) => q.Match(m => m
			.Field(f => f.Content)
			.Query(searchText));

		Action<QueryDescriptor<Blog>> titleMatchBoolPrefix = (q) => q.MatchBoolPrefix(m => m
			.Field(f => f.Content)
			.Query(searchText));

		Action<QueryDescriptor<Blog>> tagTerm = (q) => q.Term(t => t.Field(f => f.Tags).Value(searchText));
		

		if (string.IsNullOrEmpty(searchText))
		{
			listQuery.Add(matchAll);
		}

		else
		{

			listQuery.Add(matchContent);
			listQuery.Add(titleMatchBoolPrefix);
			listQuery.Add(tagTerm);
		}

		var result = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(BLOG_INDEX_NAME)
			.Size(1000).Query(q => q
				.Bool(b => b
					.Should(listQuery.ToArray()))));

		foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

		return result.Documents.ToList();
	}
}
