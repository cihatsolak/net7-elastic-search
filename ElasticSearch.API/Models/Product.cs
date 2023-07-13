namespace ElasticSearch.API.Models
{
    public class Product
    {
        //Elastic Search Auto Id. Elastic search tarafından gerçekten bir id olarak tutulmasını istiyorsak
        //Elastic tarafından primary key görevi görsün
        [PropertyName("_id")] 
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ProductFeature Feature { get; set; }
    }
}
