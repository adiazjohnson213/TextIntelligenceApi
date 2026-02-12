namespace TextIntelligenceApi.Contracts.Requests.Entities
{
    public sealed record EntitiesExtractBatchRequest(
        IReadOnlyList<EntitiesBatchDocument> Documents,
        string? DefaultLanguage = null
    );

    public sealed record EntitiesBatchDocument(
        string Id,
        string Text,
        string? Language = null
    );
}
