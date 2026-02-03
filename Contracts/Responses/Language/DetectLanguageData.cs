namespace TextIntelligenceApi.Contracts.Responses.Language
{
    public sealed record DetectLanguageData(string Language, string Iso6391Name, double ConfidenceScore);
}
