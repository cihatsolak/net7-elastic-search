namespace ElasticSearch.API.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResponseDto<ProductDto>>  InsertAsync(ProductCreateDto request)
    {
        var responseProduct = await _productRepository.InsertAsync(request.CreateProduct());
        if (responseProduct == null) 
        {
            return ResponseDto<ProductDto>.Fail("kayıt esnasında hata meydana geldi.", HttpStatusCode.InternalServerError);
        }

        return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), HttpStatusCode.Created);
    }
}
