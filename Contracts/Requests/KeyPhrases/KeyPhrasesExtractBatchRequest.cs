namespace TextIntelligenceApi.Contracts.Requests.KeyPhrases
{
    public sealed record KeyPhrasesExtractBatchRequest(
        IReadOnlyList<KeyPhrasesBatchDocument> Documents,
        string? DefaultLanguage = null
    );

    public sealed record KeyPhrasesBatchDocument(
        string Id,
        string Text,
        string? Language = null
    );
}
