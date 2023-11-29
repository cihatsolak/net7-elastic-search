namespace ElasticSearch.Web.Blogs.Extensions;

public static class ElasticSearchDIExtension
{
    public static void AddCustomElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        ElasticSetting elasticSetting = configuration.GetSection("ElasticSetting").Get<ElasticSetting>();

        var elasticsearchClientSettings = new ElasticsearchClientSettings(new Uri(elasticSetting.Uri));

        elasticsearchClientSettings.Authentication(new BasicAuthentication(elasticSetting.Username, elasticSetting.Password));

        var elasticsearchClient = new ElasticsearchClient(elasticsearchClientSettings);

        services.AddSingleton(elasticsearchClient);
    }

    private static async Task AddDefaultMappings(ElasticsearchClient client)
    {

        const string blogIndexName = "blog";

        var existsResponse = await client.Indices.ExistsAsync(blogIndexName);
        if (existsResponse.Exists)
            return;

        var createIndexResponse = await client.Indices.CreateAsync<Blog>(blogIndexName, indexDescriptor => indexDescriptor
                                         .Mappings(map => map
                                             .Properties(props => props
                                                 .Text(blog => blog.Title, textDescriptor => textDescriptor.Fields(field => field.Keyword(blog => blog.Title)))
                                                 .Text(blog => blog.Content)
                                                 .Keyword(blog => blog.UserId)
                                                 .Keyword(blog => blog.Tags)
                                                 .Date(blog => blog.Created))));
    }
}
