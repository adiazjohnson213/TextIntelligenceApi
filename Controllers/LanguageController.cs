using System.Security.Cryptography;
using Azure;
using Microsoft.AspNetCore.Mvc;
using TextIntelligenceApi.Contracts.Requests;
using TextIntelligenceApi.Contracts.Responses;
using TextIntelligenceApi.Middleware;
using TextIntelligenceApi.Services.AzureLanguage;

namespace TextIntelligenceApi.Controllers
{
    [ApiController]
    [Route("api/language")]
    public class LanguageController(AzureLanguageClient azureLanguageClient) : Controller
    {
        [HttpPost("detect")]
        public async Task<IActionResult> Detect([FromBody] DetectLanguageRequest request, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.GetCorrelationId();

            if(string.IsNullOrWhiteSpace(request?.Text))
            {
                return BadRequest(ResponseEnvelope<object>.Fail(
                    new() { new ApiError("VALIDATION_ERROR", "Text is required.", "text") },
                    correlationId));
            }

            try
            {
                var data = await azureLanguageClient.DetectLanguageAsync(request.Text, cancellationToken);
                return Ok(ResponseEnvelope<DetectLanguageData>.Ok(data, correlationId));
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                ResponseEnvelope<object>.Fail(
                    new() { new ApiError("AZURE_LANGUAGE_ERROR", ex.Message) },
                    correlationId));
            }
        }


        [HttpPost("detect:batch")]
        public async Task<IActionResult> DetectBatch()
        {
            return View();
        }
    }
}
