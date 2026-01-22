using Microsoft.AspNetCore.Mvc;

namespace TextIntelligenceApi.Controllers
{
    [ApiController]
    [Route("api/entities")]
    public class EntitiesController : Controller
    {
        [HttpPost("detect")]
        public async Task<IActionResult> Detect()
        {
            return View();
        }


        [HttpPost("detect:batch")]
        public async Task<IActionResult> DetectBatch()
        {
            return View();
        }
    }
}
