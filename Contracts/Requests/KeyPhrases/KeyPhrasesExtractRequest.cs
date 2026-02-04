namespace TextIntelligenceApi.Contracts.Requests.KeyPhrases
{
    public sealed record KeyPhrasesExtractRequest(
        string Text,
        string? Language = null
    );
}
