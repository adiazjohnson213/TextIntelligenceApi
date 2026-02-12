using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Options;
using TextIntelligenceApi.Common;
using TextIntelligenceApi.Contracts.Responses;
using TextIntelligenceApi.Middleware;
using TextIntelligenceApi.Services.AzureLanguage;

var builder = WebApplication.CreateBuilder(args);

//Bind configuration settings
builder.Services.Configure<AzureLanguageOptions>(
    builder.Configuration.GetSection("Language"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(sp =>
{
    var opt = sp.GetRequiredService<IOptions<AzureLanguageOptions>>().Value;
    return new TextAnalyticsClient(new Uri(opt.Endpoint), new AzureKeyCredential(opt.ApiKey));
});
builder.Services.AddScoped<AzureLanguageClient>();
builder.Services.AddScoped<SentimentAnalysisClient>();
builder.Services.AddScoped<KeyPhrasesClient>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseExceptionHandler(appErr =>
{
    appErr.Run(async context =>
    {
        var correlationId = context.GetCorrelationId();
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var body = ResponseEnvelope<object>.Fail(
            new()
            {
                new ApiError("UNHANDLED_ERROR", "An unexpected error occurred. Please try again later.")
            },
            correlationId);

        await context.Response.WriteAsJsonAsync(body);
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
