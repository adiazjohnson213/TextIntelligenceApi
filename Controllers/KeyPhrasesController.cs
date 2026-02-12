using Azure;
using Microsoft.AspNetCore.Mvc;
using TextIntelligenceApi.Contracts.Requests.KeyPhrases;
using TextIntelligenceApi.Contracts.Responses;
using TextIntelligenceApi.Contracts.Responses.KeyPhrases;
using TextIntelligenceApi.Middleware;
using TextIntelligenceApi.Services.AzureLanguage;

namespace TextIntelligenceApi.Controllers
{
    [ApiController]
    [Route("api/keyphrases")]
    public class KeyPhrasesController(KeyPhrasesClient keyPhrasesClient) : Controller
    {
        [HttpPost("extract")]
        public async Task<IActionResult> Extract([FromBody] KeyPhrasesExtractRequest request, CancellationToken cancellationToken)
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
                var data = await keyPhrasesClient.ExtractAsync(request, cancellationToken);
                return Ok(ResponseEnvelope<KeyPhrasesExtractResponse>.Ok(data, correlationId));
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                ResponseEnvelope<object>.Fail(
                    new() { new ApiError("AZURE_KEYPHRASES_ERROR", ex.Message) },
                    correlationId));
            }
        }


        [HttpPost("extract:batch")]
        public async Task<IActionResult> ExtractBatch([FromBody] KeyPhrasesExtractBatchRequest request, CancellationToken cancellationToken)
        {
            var correlationId = HttpContext.GetCorrelationId();

            if (request is null)
            {
                return BadRequest(ResponseEnvelope<object>.Fail(
                    new() { new ApiError("VALIDATION_ERROR", "Request body is required.", "request") },
                    correlationId));
            }

            if (request.Documents is null || request.Documents.Count == 0)
            {
                return BadRequest(ResponseEnvelope<object>.Fail(
                    new() { new ApiError("VALIDATION_ERROR", "Documents is required and must not be empty.", "documents") },
                    correlationId));
            }

            try
            {
                var data = await keyPhrasesClient.ExtractBatchAsync(request, cancellationToken);
                return Ok(ResponseEnvelope<KeyPhrasesExtractBatchResponse>.Ok(data, correlationId));
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                ResponseEnvelope<object>.Fail(
                    new() { new ApiError("AZURE_KEYPHRASES_ERROR", ex.Message) },
                    correlationId));
            }
        }
    }
}
