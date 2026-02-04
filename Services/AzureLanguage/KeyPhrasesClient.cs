using Azure.AI.TextAnalytics;
using TextIntelligenceApi.Contracts.Requests.KeyPhrases;
using TextIntelligenceApi.Contracts.Responses;
using TextIntelligenceApi.Contracts.Responses.KeyPhrases;

namespace TextIntelligenceApi.Services.AzureLanguage
{
    public sealed class KeyPhrasesClient(TextAnalyticsClient client)
    {
        public async Task<KeyPhrasesExtractResponse> ExtractAsync(
                            KeyPhrasesExtractRequest request, CancellationToken ct)
        {
            var response = await client.ExtractKeyPhrasesAsync(
                request.Text,
                request.Language,
                ct);

            return new KeyPhrasesExtractResponse(response.Value, request.Language);
        }

        public async Task<KeyPhrasesExtractBatchResponse> ExtractBatchAsync(
                            KeyPhrasesExtractBatchRequest request, CancellationToken ct)
        {
            var inputs = request.Documents
            .Select(d => new TextDocumentInput(d.Id, d.Text)
            {
                Language = d.Language ?? request.DefaultLanguage
            })
            .ToList();

            var response = await client.ExtractKeyPhrasesBatchAsync(inputs, cancellationToken: ct);

            return new KeyPhrasesExtractBatchResponse(
                response.Value.Select(keyPhrasesResult =>
                {
                    if(keyPhrasesResult.HasError)
                    {
                        return new KeyPhrasesBatchItem(
                            keyPhrasesResult.Id, null,
                            new() 
                            { 
                                new ApiError(
                                    keyPhrasesResult.Error.ErrorCode.ToString(), 
                                    keyPhrasesResult.Error.Message
                                ) 
                            }
                        );
                    }

                    return new KeyPhrasesBatchItem(
                        keyPhrasesResult.Id,
                        new KeyPhrasesExtractResponse(
                            keyPhrasesResult.KeyPhrases.ToArray(), 
                            request.Documents.FirstOrDefault(x => x.Id == keyPhrasesResult.Id)?.Language ?? request.DefaultLanguage
                        ),
                        new()
                    );
                }).ToList());
        }
    }
}
