namespace TextIntelligenceApi.Contracts.Requests
{
    public sealed record DetectLanguageData(string Language, string Iso6391Name, double ConfidenceScore);
}
