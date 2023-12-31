﻿namespace ElasticSearch.API.Nest.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController : BaseController
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> Insert(ProductCreateDto request)
    {
        var response = await _productService.InsertAsync(request);
        return CreateActionResult(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _productService.GetAllAsync();
        return CreateActionResult(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _productService.GetByIdAsync(id);
        return CreateActionResult(response);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var response = await _productService.GetByNameAsync(name);
        return CreateActionResult(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto request)
    {
        var response = await _productService.UpdateAsync(request);
        return CreateActionResult(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _productService.DeleteAsync(id);
        return CreateActionResult(response);
    }
}