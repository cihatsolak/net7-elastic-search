namespace ElasticSearch.API.Controllers;

[Route("api/[controller]")]
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
}
