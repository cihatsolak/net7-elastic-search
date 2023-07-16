namespace ElasticSearch.API.ElasticClientsSearch.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public ProductFeature Feature { get; set; }

        public ProductDto CreateDto()
        {
            var productDto = new ProductDto()
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Stock = Stock,
            };

            if (Feature == null)
            {
                return productDto;
            }

            productDto.Feature = new ProductFeatureDto
            {
                Width = Feature.Width,
                Height = Feature.Height,
                Color = Feature.Color.ToString()
            };

            return productDto;
        }
    }
}
