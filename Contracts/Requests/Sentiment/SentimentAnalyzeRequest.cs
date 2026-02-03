namespace TextIntelligenceApi.Contracts.Requests.Sentiment
{
    public sealed record SentimentAnalyzeRequest(
        string Text,
        string? Language = null,
        bool IncludeOpinionMining = false
    );
}
