using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("")]
    public class HealthController : Controller
    {
        [HttpHead]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}