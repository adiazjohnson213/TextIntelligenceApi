namespace TextIntelligenceApi.Contracts.Requests.Language
{
    public sealed record DetectLanguageBatchRequest(List<DetectLanguageBatchItem> Items);

    public sealed record DetectLanguageBatchItem(string Id, string Text);
}
