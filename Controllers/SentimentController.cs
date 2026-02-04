using Azure;
using Microsoft.AspNetCore.Mvc;
using TextIntelligenceApi.Contracts.Requests.Sentiment;
using TextIntelligenceApi.Contracts.Responses;
using TextIntelligenceApi.Contracts.Responses.Sentiment;
using TextIntelligenceApi.Middleware;
using TextIntelligenceApi.Services.AzureLanguage;

namespace TextIntelligenceApi.Controllers
{
    [ApiController]
    [Route("api/sentiment")]
    public class SentimentController(SentimentAnalysisClient sentimentAnalysisClient) : Controller
    {
        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] SentimentAnalyzeRequest request, CancellationToken cancellationToken)
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
                var data = await sentimentAnalysisClient.SentimentAnalyzeAsync(request, cancellationToken);
                return Ok(ResponseEnvelope<SentimentAnalyzeData>.Ok(data, correlationId));
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                ResponseEnvelope<object>.Fail(
                    new() { new ApiError("AZURE_SENTIMENT_ERROR", ex.Message) },
                    correlationId));
            }
        }


        [HttpPost("analyze:batch")]
        public async Task<IActionResult> AnalyzeBatch([FromBody] SentimentAnalyzeBatchRequest request, CancellationToken cancellationToken)
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
                var data = await sentimentAnalysisClient.SentimentAnalyzeBatchAsync(request, cancellationToken);
                return Ok(ResponseEnvelope<SentimentAnalyzeBatchData>.Ok(data, correlationId));
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                    ResponseEnvelope<object>.Fail(
                        new() { new ApiError("AZURE_SENTIMENT_ERROR", ex.Message) },
                        correlationId));
            }
        }
    }
}
