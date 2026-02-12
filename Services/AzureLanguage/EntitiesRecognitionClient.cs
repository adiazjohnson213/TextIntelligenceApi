using Azure.AI.TextAnalytics;
using TextIntelligenceApi.Contracts.Requests.Entities;
using TextIntelligenceApi.Contracts.Responses;
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

        public async Task<EntitiesExtractBatchResponse> ExtractBatchAsync(
            EntitiesExtractBatchRequest request, CancellationToken ct)
        {
            var inputs = request.Documents
            .Select(d => new TextDocumentInput(d.Id, d.Text)
            {
                Language = d.Language ?? request.DefaultLanguage
            })
            .ToList();

            var response = await client.RecognizeEntitiesBatchAsync(inputs, cancellationToken: ct);

            return new EntitiesExtractBatchResponse(
                response.Value.Select(entitiesResult =>
                {
                    if (entitiesResult.HasError)
                    {
                        return new EntitiesBatchItem(
                            entitiesResult.Id, null,
                            new()
                            {
                                new ApiError(
                                    entitiesResult.Error.ErrorCode.ToString(),
                                    entitiesResult.Error.Message
                                )
                            }
                        );
                    }
                    return new EntitiesBatchItem(
                        entitiesResult.Id,
                        new EntitiesExtractResponse(
                            entitiesResult.Entities.Select(e => new RecognizedEntityItem(
                                e.Text,
                                e.Category.ToString(),
                                e.SubCategory,
                                e.ConfidenceScore,
                                e.Offset,
                                e.Length
                            )).ToList()
                        ),
                        new()
                    );
                }).ToList());
        }
    }
}
