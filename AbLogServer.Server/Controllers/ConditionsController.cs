using ab.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace AbLogServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConditionsController : ControllerBase
    {
        private readonly ILogger<ConditionsController> _logger;

        public ConditionsController(ILogger<ConditionsController> logger)
        {
            _logger = logger;
        }
    }
}