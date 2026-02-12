using Azure.AI.TextAnalytics;
using TextIntelligenceApi.Contracts.Requests.Entities;
using TextIntelligenceApi.Contracts.Responses.Entities;

namespace TextIntelligenceApi.Services.AzureLanguage
{
    public class EntitiesRecognitionClient(TextAnalyticsClient client)
    {
        public async Task<EntitiesExtractResponse> ExtractAsync(
            EntitiesExtractRequest request, CancellationToken ct)
        {
            var response = await client.RecognizeEntitiesAsync(
                request.Text,
                request.Language,
                ct);

            return new EntitiesExtractResponse(
                response.Value.Select(e => new RecognizedEntityItem(
                    e.Text,
                    e.Category.ToString(),
                    e.SubCategory,
                    e.ConfidenceScore,
                    e.Offset,
                    e.Length
                )).ToList()
            );
        }
    }
}
