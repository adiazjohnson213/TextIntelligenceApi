namespace TextIntelligenceApi.Contracts.Responses.KeyPhrases
{
    public sealed record KeyPhrasesExtractBatchResponse(
    IReadOnlyList<KeyPhrasesBatchItem> Items
);

    public sealed record KeyPhrasesBatchItem(
        string Id,
        KeyPhrasesExtractResponse? Result,
        List<ApiError> Errors
    );
}
