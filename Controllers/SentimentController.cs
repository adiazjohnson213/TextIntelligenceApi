using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TextIntelligenceApi.Controllers
{
    [ApiController]
    [Route("api/sentiment")]
    public class SentimentController : Controller
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
