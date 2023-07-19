namespace ElasticSearch.Web.Blog.Settings;

public sealed record ElasticSetting
{
    public string Uri { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}
