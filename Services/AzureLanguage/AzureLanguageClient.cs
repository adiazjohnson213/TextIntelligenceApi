using Azure.AI.TextAnalytics;
using TextIntelligenceApi.Contracts.Requests;

namespace TextIntelligenceApi.Services.AzureLanguage
{
    public sealed class AzureLanguageClient(TextAnalyticsClient client)
    {
        public async Task<DetectLanguageData> DetectLanguageAsync(string text, CancellationToken ct)
        {
            var response = await client.DetectLanguageAsync(text, cancellationToken: ct);
            var language = response.Value;

            return new DetectLanguageData(language.Name, language.Iso6391Name, language.ConfidenceScore);
        }
    }
}
