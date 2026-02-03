namespace TextIntelligenceApi.Contracts.Responses.Language
{
    public sealed record DetectLanguageBatchData(List<DetectLanguageBatchResultItem> Items);

    public sealed record DetectLanguageBatchResultItem(
        string Id,
        DetectLanguageData? Result,
        List<ApiError> Errors
    );
}
