using TextIntelligenceApi.Contracts.Responses;
using TextIntelligenceApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Bind configuration settings

// Add services to the container.
builder.Services.AddControllers();

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
