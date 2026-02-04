namespace TextIntelligenceApi.Contracts.Responses.KeyPhrases
{
    public sealed record KeyPhrasesExtractResponse(
        IReadOnlyList<string> KeyPhrases,
        string? Language = null
    );
}
