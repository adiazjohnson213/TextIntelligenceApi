namespace TextIntelligenceApi.Contracts.Responses.Entities
{
    public sealed record EntitiesExtractBatchResponse(
        IReadOnlyList<EntitiesBatchItem> Items
    );

    public sealed record EntitiesBatchItem(
        string Id,
        EntitiesExtractResponse? Result,
        List<ApiError> Errors
    );
}
