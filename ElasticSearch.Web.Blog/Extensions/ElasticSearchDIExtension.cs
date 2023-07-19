namespace ElasticSearch.Web.Blog.Extensions;

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
}
