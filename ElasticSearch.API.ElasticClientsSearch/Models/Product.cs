using ElasticSearch.API.ElasticClientsSearch.DTOs;
using ElasticSearch.API.Nest.DTOs;

namespace ElasticSearch.API.ElasticClientsSearch.Models
{
    public class Product
    {
        //Elastic Search Auto Id. Elastic search tarafından gerçekten bir id olarak tutulmasını istiyorsak
        //Elastic tarafından primary key görevi görsün
        [PropertyName("_id")]
        public string Id { get; set; }
        public string Name { get; set; }

        [Number(NumberType.Double)]
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
