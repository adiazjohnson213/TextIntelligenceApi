namespace TextIntelligenceApi.Contracts.Responses
{
    public sealed record ResponseEnvelope<T>(bool Success, T? Data, List<ApiError> Errors, string CorrelationId)
    {
        public static ResponseEnvelope<T> Ok(T data, string correlationId) =>
            new(true, data, new(), correlationId);
        public static ResponseEnvelope<T> Fail(List<ApiError> errors, string correlationId) =>
            new(false, default, errors, correlationId);
    }
}
