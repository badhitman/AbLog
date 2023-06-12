using ab.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLib;

namespace AbLogServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentionsController : ControllerBase
    {
        private readonly ILogger<ContentionsController> _logger;

        public ContentionsController(ILogger<ContentionsController> logger)
        {
            _logger = logger;
        }
    }
}