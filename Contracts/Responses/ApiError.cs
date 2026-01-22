namespace TextIntelligenceApi.Contracts.Responses
{
    public sealed record ApiError(string Code, string Message, string? Target = null);
}
