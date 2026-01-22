namespace TextIntelligenceApi.Contracts.Responses
{
    public sealed record DetectLanguageData(string Language, string Iso6391Name, double ConfidenceScore);
}
