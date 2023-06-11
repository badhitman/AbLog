using Microsoft.AspNetCore.Mvc;

namespace AbLogServer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HardwaresController : ControllerBase
    {
        private readonly ILogger<HardwaresController> _logger;

        public HardwaresController(ILogger<HardwaresController> logger)
        {
            _logger = logger;
        }
    }
}