namespace ElasticSearch.API.ElasticClientsSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECommerceController : BaseController
    {
        private readonly ECommerceRepository _repository;

        public ECommerceController(ECommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{customerFirstName}")]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            var response = await _repository.TermQueryAsync(customerFirstName);
            return Ok(response);
        }
    }
}
