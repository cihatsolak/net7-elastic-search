namespace ElasticSearch.API.ElasticClientsSearch.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : BaseController
    {
        private readonly ECommerceRepository _repository;

        public ECommerceController(ECommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            var response = await _repository.TermQuery(customerFirstName);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {
            var response = await _repository.TermsQuery(customerFirstNameList);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            var response = await _repository.PrefixQuery(customerFullName);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            var response = await _repository.RangeQuery(fromPrice, toPrice);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> MatchAllQuery()
        {
            var response = await _repository.MatchAllQuery();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> PaginationQuery(int pageIndex, int pageSize)
        {
            var response = await _repository.PaginationQuery(pageIndex, pageSize);
            return Ok(response);
        }
    }
}
