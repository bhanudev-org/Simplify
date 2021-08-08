namespace Simplify.Web.Api.V2
{
    public class MetaController : ControllerBaseV2
    {
        private readonly ILogger<MetaController> _logger;

        public MetaController(ILogger<MetaController> logger) => _logger = logger;

        public IActionResult Get()
        {
            _logger.LogInformation("[API] [META] [GET] [{@headers}] : Called", Request.Headers);

            return Ok(new MetaViewModel());
        }
    }
}