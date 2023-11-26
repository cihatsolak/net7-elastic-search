namespace ElasticSearch.API.Nest.Extensions;

public static class ElasticSearchDIExtension
{
    public static void AddCustomElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        ElasticSetting elasticSetting = configuration.GetSection("ElasticSetting").Get<ElasticSetting>();

        //Gerçek hayatta birden fazla node ile çalışılabilir ama docker da tek node çalışıyoruz.
        var singleNodeConnectionPool = new SingleNodeConnectionPool(new Uri(elasticSetting.Uri));
        var connectionString = new ConnectionSettings(singleNodeConnectionPool).DisableDirectStreaming();
        
        //connectionString = connectionString.BasicAuthentication(elasticSetting.Username, elasticSetting.Password);

        var elasticClient = new ElasticClient(connectionString);

        services.AddSingleton(elasticClient);
    }
}
