namespace TextIntelligenceApi.Contracts.Responses.Entities
{
    public sealed record EntitiesExtractResponse(
        IReadOnlyList<RecognizedEntityItem> Entities
    );

    public sealed record RecognizedEntityItem(
        string Text,
        string Category,
        string? SubCategory,
        double ConfidenceScore,
        int Offset,
        int Length
    );
}
