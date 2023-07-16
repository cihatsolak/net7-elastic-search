namespace ElasticSearch.API.DTOs
{
    //record class gibidir.
    public record ProductCreateDto
    {
        public string Name { get; init; }
        public float Price { get; init; }
        public int Stock { get; init; }
        public ProductFeatureDto Feature { get; init; }

        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = Stock, 
                Feature = new ProductFeature
                {
                    Color = (EColor)int.Parse(Feature.Color),
                    Height = Feature.Height,
                    Width = Feature.Width
                }
            };
        }
    }
}
