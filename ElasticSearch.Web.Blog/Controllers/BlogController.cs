﻿namespace ElasticSearch.Web.Blogs.Controllers;

public class BlogController : Controller
{
    private readonly BlogService _blogService;

    public BlogController(BlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public IActionResult Save()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(BlogCreateViewModel blog)
    {
        bool succedeed = await _blogService.InsertAsync(blog);
        if (!succedeed)
        {
            TempData["result"] = "The blog could not be added.";
            return RedirectToAction(nameof(BlogController.Save));
        }

        TempData["result"] = "The blog has been successfully added.";
        return RedirectToAction(nameof(BlogController.Save));
    }
}
