namespace ElasticSearch.Web.Blogs.Services;

public class BlogService
{
    //Dependency Inversion / Inversion Of Control => Dependency Injection DP
    private readonly BlogRepository _blogRepository;

    public BlogService(BlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<bool> InsertAsync(BlogCreateViewModel model)
    {
        var blog = new Blog
        {
            Title = model.Title,
            Content = model.Content,
            Tags = model.Tags.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            UserId = Guid.NewGuid()
        };

        var isCreatedBlog = await _blogRepository.InsertAsync(blog);

        return isCreatedBlog != null;
    }

    public async Task<List<Blog>> SearchAsync(string searchText)
    {
        return await _blogRepository.SearchAsync(searchText);
    }
}
