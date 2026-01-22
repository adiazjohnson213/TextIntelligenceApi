using Azure.AI.TextAnalytics;
using TextIntelligenceApi.Contracts.Responses;

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

        public async Task<List<DetectLanguageBatchResultItem>> DetectLanguageBatchAsync(
            IEnumerable<DetectLanguageInput> inputs, CancellationToken ct)
        {
            var res = await client.DetectLanguageBatchAsync(inputs, cancellationToken: ct);

            return res.Value.Select(doc =>
            {
                if (doc.HasError)
                    return new DetectLanguageBatchResultItem(
                        doc.Id, null,
                        new() { new ApiError("AZURE_LANGUAGE_DOC_ERROR", doc.Error.Message) }
                    );

                var lang = doc.PrimaryLanguage;
                return new DetectLanguageBatchResultItem(
                    doc.Id,
                    new DetectLanguageData(lang.Name, lang.Iso6391Name, lang.ConfidenceScore),
                    new()
                );
            }).ToList();
        }
    }
}
