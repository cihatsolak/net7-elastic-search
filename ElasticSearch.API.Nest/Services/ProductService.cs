namespace ElasticSearch.API.Nest.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ResponseDto<ProductDto>> InsertAsync(ProductCreateDto request)
    {
        var responseProduct = await _productRepository.InsertAsync(request.CreateProduct());
        if (responseProduct == null)
        {
            return ResponseDto<ProductDto>.Fail("An error occurred while inserting.", HttpStatusCode.InternalServerError);
        }

        return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), HttpStatusCode.Created);
    }

    public async Task<ResponseDto<ImmutableList<ProductDto>>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        if (!products.Any())
        {
            return ResponseDto<ImmutableList<ProductDto>>.Fail("product not found.", HttpStatusCode.NotFound);
        }

        List<ProductDto> productDTOs = new();

        foreach (var product in products)
        {
            ProductDto productDto = new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Created = product.Created,
                Updated = product.Updated
            };

            if (product.Feature is not null)
            {
                productDto.Feature = new ProductFeatureDto
                {
                    Width = product.Feature.Width,
                    Height = product.Feature.Height,
                    Color = product.Feature.Color.ToString()
                };
            }

            productDTOs.Add(productDto);
        }

        return ResponseDto<ImmutableList<ProductDto>>.Success(productDTOs.ToImmutableList(), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
    {
        var hasProduct = await _productRepository.GetByIdAsync(id);
        if (hasProduct is null)
        {
            return ResponseDto<ProductDto>.Fail("product not found.", HttpStatusCode.NotFound);
        }

        return ResponseDto<ProductDto>.Success(hasProduct.CreateDto(), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<ImmutableList<ProductDto>>> GetByNameAsync(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        var products = await _productRepository.GetByNameAsync(name);
        if (!products.Any())
        {
            return ResponseDto<ImmutableList<ProductDto>>.Fail("product not found.", HttpStatusCode.NotFound);
        }

        List<ProductDto> productDTOs = new();

        foreach (var product in products)
        {
            ProductDto productDto = new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Created = product.Created,
                Updated = product.Updated
            };

            if (product.Feature is not null)
            {
                productDto.Feature = new ProductFeatureDto
                {
                    Width = product.Feature.Width,
                    Height = product.Feature.Height,
                    Color = product.Feature.Color.ToString()
                };
            }

            productDTOs.Add(productDto);
        }

        return ResponseDto<ImmutableList<ProductDto>>.Success(productDTOs.ToImmutableList(), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productUpdateDto)
    {
        bool succeeded = await _productRepository.UpdateAsync(productUpdateDto);
        if (!succeeded)
        {
            return ResponseDto<bool>.Fail("The product could not be updated.", HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(succeeded, HttpStatusCode.NoContent);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(string id)
    {
        var deleteResponse = await _productRepository.DeleteAsync(id);

        if (deleteResponse.Result == Result.NotFound) //Ürün db'de yok mu?
        {
            return ResponseDto<bool>.Fail("The item to be deleted was not found.", HttpStatusCode.NotFound);
        }

        if (!deleteResponse.IsValid)
        {
            _logger.LogError(deleteResponse.OriginalException, deleteResponse.ServerError.Error.ToString()); //error log

            return ResponseDto<bool>.Fail("The product could not be deleted.", HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(deleteResponse.IsValid, HttpStatusCode.NoContent);
    }
}