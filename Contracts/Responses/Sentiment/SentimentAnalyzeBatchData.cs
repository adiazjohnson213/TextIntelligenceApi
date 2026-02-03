namespace TextIntelligenceApi.Contracts.Responses.Sentiment
{
    public sealed record SentimentAnalyzeBatchData(
        IReadOnlyList<SentimentBatchItem> Items
    );

    public sealed record SentimentBatchItem(
        string Id,
        SentimentAnalyzeData? Result,
        List<ApiError> Errors
    );
}
