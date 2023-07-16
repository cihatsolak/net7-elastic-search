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

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {
            var response = await _repository.TermsQueryAsync(customerFirstNameList);
            return Ok(response);
        }

    }
}
