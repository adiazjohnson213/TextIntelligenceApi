namespace TextIntelligenceApi.Contracts.Requests.Sentiment
{
    public sealed record SentimentAnalyzeBatchRequest(
        IReadOnlyList<SentimentBatchDocument> Documents,
        string? DefaultLanguage = null,
        bool IncludeOpinionMining = false
    );

    public sealed record SentimentBatchDocument(
        string Id,
        string Text,
        string? Language = null
    );
}
