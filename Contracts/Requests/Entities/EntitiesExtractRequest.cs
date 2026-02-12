namespace TextIntelligenceApi.Contracts.Requests.Entities
{
    public sealed record EntitiesExtractRequest(
        string Text,
        string? Language = null
    );
}
