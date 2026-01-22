using Microsoft.AspNetCore.Mvc;

namespace TextIntelligenceApi.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController : Controller
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
