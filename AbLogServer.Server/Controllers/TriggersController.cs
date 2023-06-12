using ab.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace AbLogServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TriggersController : ControllerBase
    {
        private readonly ILogger<TriggersController> _logger;

        public TriggersController(ILogger<TriggersController> logger)
        {
            _logger = logger;
        }
    }
}