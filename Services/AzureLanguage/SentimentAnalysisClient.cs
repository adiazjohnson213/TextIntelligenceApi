using Azure.AI.TextAnalytics;
using TextIntelligenceApi.Contracts.Requests.Sentiment;
using TextIntelligenceApi.Contracts.Responses.Sentiment;
using SentenceSentiment = TextIntelligenceApi.Contracts.Responses.Sentiment.SentenceSentiment;
using SentimentConfidenceScores = TextIntelligenceApi.Contracts.Responses.Sentiment.SentimentConfidenceScores;

namespace TextIntelligenceApi.Services.AzureLanguage
{
    public sealed class SentimentAnalysisClient(TextAnalyticsClient client)
    {
        public async Task<SentimentAnalyzeData> SentimentAnalyzeAsync(
        SentimentAnalyzeRequest request, CancellationToken ct)
        {
            var analyzeOptions = new AnalyzeSentimentOptions
            {
                IncludeOpinionMining = request.IncludeOpinionMining
            };

            var response = await client.AnalyzeSentimentAsync(
                request.Text,
                request.Language,
                analyzeOptions,
                ct);

            var doc = response.Value;

            return new SentimentAnalyzeData(
                Sentiment: doc.Sentiment.ToString(),
                ConfidenceScores: new SentimentConfidenceScores(
                    Positive: doc.ConfidenceScores.Positive,
                    Neutral: doc.ConfidenceScores.Neutral,
                    Negative: doc.ConfidenceScores.Negative
                ),
                Sentences: doc.Sentences.Select(s => new SentenceSentiment(
                    Text: s.Text,
                    Sentiment: s.Sentiment.ToString(),
                    ConfidenceScores: new SentimentConfidenceScores(
                        Positive: s.ConfidenceScores.Positive,
                        Neutral: s.ConfidenceScores.Neutral,
                        Negative: s.ConfidenceScores.Negative
                    ),
                    Offset: s.Offset,
                    Length: s.Length
                )).ToArray(),
                Language: request.Language
            );
        }

        public async Task<SentimentAnalyzeBatchData> SentimentAnalyzeBatchAsync(
        SentimentAnalyzeBatchRequest request,
        CancellationToken ct)
        {
            var analyzeOptions = new AnalyzeSentimentOptions
            {
                IncludeOpinionMining = request.IncludeOpinionMining
            };

            var inputs = request.Documents
            .Select(d => new TextDocumentInput(d.Id, d.Text)
            {
                Language = d.Language ?? request.DefaultLanguage
            })
            .ToList();

            var response = await client.AnalyzeSentimentBatchAsync(inputs, analyzeOptions, ct);

            return new SentimentAnalyzeBatchData(
                response.Value.Select(sentimentResult =>
                {
                    if (sentimentResult.HasError)
                    {
                        return new SentimentBatchItem(
                            Id: sentimentResult.Id,
                            Result: null,
                            Errors: new List<Contracts.Responses.ApiError>
                            {
                                new Contracts.Responses.ApiError(
                                    Code: sentimentResult.Error.ErrorCode.ToString(),
                                    Message: sentimentResult.Error.Message
                                )
                            }
                        );
                    }

                    var documentSentiment = sentimentResult.DocumentSentiment;

                    return new SentimentBatchItem(
                        sentimentResult.Id,
                        new SentimentAnalyzeData(
                            Sentiment: documentSentiment.ToString(),
                            ConfidenceScores: new SentimentConfidenceScores(
                                Positive: documentSentiment.ConfidenceScores.Positive,
                                Neutral: documentSentiment.ConfidenceScores.Neutral,
                                Negative: documentSentiment.ConfidenceScores.Negative
                            ),
                            Sentences: documentSentiment.Sentences.Select(s => new SentenceSentiment(
                                Text: s.Text,
                                Sentiment: s.Sentiment.ToString(),
                                ConfidenceScores: new SentimentConfidenceScores(
                                    Positive: s.ConfidenceScores.Positive,
                                    Neutral: s.ConfidenceScores.Neutral,
                                    Negative: s.ConfidenceScores.Negative
                                ),
                                Offset: s.Offset,
                                Length: s.Length
                            )).ToArray(),
                            Language: request.Documents.FirstOrDefault(x => x.Id == sentimentResult.Id)?.Language ?? request.DefaultLanguage
                        ),
                        new()
                    );
                }).ToList());
        }
    }
}
