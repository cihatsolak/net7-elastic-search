namespace ElasticSearch.Web.Blogs.Repositories;

public sealed class BlogRepository
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string BLOG_INDEX_NAME = "blog";

    public BlogRepository(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task<Blog> InsertAsync(Blog blog)
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

		Action<QueryDescriptor<Blog>> matchAllQuery = (query) => query.MatchAll();

		Action<QueryDescriptor<Blog>> matchContentQuery = (query) => query.Match(match => match
																			 .Field(field => field.Content)
																				.Query(searchText));

		Action<QueryDescriptor<Blog>> titleMatchBoolPrefixQuery = (query) => query.MatchBoolPrefix(match => match
																					 .Field(field => field.Content)
																						.Query(searchText));

		Action<QueryDescriptor<Blog>> tagTermQuery = (query) => query.Term(term => term
																		.Field(field => field.Tags)
																			.Value(searchText));
		

		if (string.IsNullOrEmpty(searchText))
		{
			listQuery.Add(matchAllQuery);
		}
		else
		{

			listQuery.Add(matchContentQuery);
			listQuery.Add(titleMatchBoolPrefixQuery);
			listQuery.Add(tagTermQuery);
		}

		var result = await _elasticsearchClient.SearchAsync<Blog>(search => search.Index(BLOG_INDEX_NAME)
					.Size(1000)
						.Query(query => query
							.Bool(b => b
								.Should(listQuery.ToArray()))));

		foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

		return result.Documents.ToList();
	}
}
