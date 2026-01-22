using Azure;
using Azure.AI.TextAnalytics;
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
        public async Task<IActionResult> DetectBatch([FromBody] DetectLanguageBatchRequest request, CancellationToken cancellationToken)
        {
            var cid = HttpContext.GetCorrelationId();

            if (request?.Items is null || request.Items.Count == 0)
                return BadRequest(ResponseEnvelope<object>.Fail(
                    new() { new ApiError("VALIDATION_ERROR", "Items is required.", "items") }, cid));

            var results = new List<DetectLanguageBatchResultItem>();
            var validInputs = new List<DetectLanguageInput>();

            foreach (var it in request.Items)
            {
                if (string.IsNullOrWhiteSpace(it?.Id))
                {
                    results.Add(new DetectLanguageBatchResultItem(
                        Id: it?.Id ?? "",
                        Result: null,
                        Errors: new() { new ApiError("VALIDATION_ERROR", "Id is required.", "items.id") }
                    ));
                    continue;
                }

                if (string.IsNullOrWhiteSpace(it.Text))
                {
                    results.Add(new DetectLanguageBatchResultItem(
                        it.Id, null,
                        new() { new ApiError("VALIDATION_ERROR", "Text is required.", $"items[{it.Id}].text") }
                    ));
                    continue;
                }

                validInputs.Add(new DetectLanguageInput(it.Id, it.Text));
            }

            try
            {
                if (validInputs.Count > 0)
                {
                    var azureItems = await azureLanguageClient.DetectLanguageBatchAsync(validInputs, cancellationToken);
                    results.AddRange(azureItems);
                }

                var data = new DetectLanguageBatchData(results);
                return Ok(ResponseEnvelope<DetectLanguageBatchData>.Ok(data, cid)); // 200 siempre si request global válido
            }
            catch (RequestFailedException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway,
                    ResponseEnvelope<object>.Fail(new() { new ApiError("AZURE_LANGUAGE_ERROR", ex.Message) }, cid));
            }
        }
    }
}
