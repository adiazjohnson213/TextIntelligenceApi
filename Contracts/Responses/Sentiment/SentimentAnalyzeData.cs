namespace TextIntelligenceApi.Contracts.Responses.Sentiment
{
    public sealed record SentimentAnalyzeData(
        string Sentiment,
        SentimentConfidenceScores ConfidenceScores,
        IReadOnlyList<SentenceSentiment> Sentences,
        string? Language = null
    );

    public sealed record SentenceSentiment(
        string Text,
        string Sentiment,
        SentimentConfidenceScores ConfidenceScores,
        int Offset,
        int Length
    );

    public sealed record SentimentConfidenceScores(
        double Positive,
        double Neutral,
        double Negative
    );
}
