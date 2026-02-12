using Azure;
using Microsoft.AspNetCore.Mvc;
using TextIntelligenceApi.Contracts.Requests.Entities;
using TextIntelligenceApi.Contracts.Responses;
using TextIntelligenceApi.Contracts.Responses.Entities;
using TextIntelligenceApi.Middleware;
using TextIntelligenceApi.Services.AzureLanguage;

namespace TextIntelligenceApi.Controllers
{
    [ApiController]
    [Route("api/entities")]
    public class EntitiesController(EntitiesRecognitionClient entitiesRecognitionClient) : Controller
    {
        [HttpPost("extract")]
        public async Task<IActionResult> Extract([FromBody] EntitiesExtractRequest request, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.GetCorrelationId();

            if (request is null)
            {
                return BadRequest(ResponseEnvelope<object>.Fail(
                    new() { new ApiError("VALIDATION_ERROR", "Request body is required.", "request") },
                    correlationId));
            }

            if (string.IsNullOrWhiteSpace(request?.Text))
            {
                return BadRequest(ResponseEnvelope<object>.Fail(
                    new() { new ApiError("VALIDATION_ERROR", "Text is required.", "text") },
                    correlationId));
            }

            try
            {
                var data = await entitiesRecognitionClient.ExtractAsync(request, cancellationToken);
                return Ok(ResponseEnvelope<EntitiesExtractResponse>.Ok(data, correlationId));
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                    ResponseEnvelope<object>.Fail(
                        new() { new ApiError("AZURE_ENTITIES_ERROR", ex.Message) },
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
