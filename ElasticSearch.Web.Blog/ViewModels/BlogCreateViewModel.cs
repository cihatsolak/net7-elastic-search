namespace ElasticSearch.Web.Blogs.ViewModels;

public class BlogCreateViewModel
{
    [Required]
    [DisplayName("Blog Title")]
    public string Title { get; set; }

    [Required]
    [DisplayName("Blog Content")]
    public string Content { get; set; }

    [Required]
    [DisplayName("Blog Tags")]
    public string Tags { get; set; }
}
