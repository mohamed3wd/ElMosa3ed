using ElMosa3ed.Api.Services;
using ElMosa3ed.Api.Data;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy for Chrome Extension
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowChromeExtension", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Allow all origins including chrome-extension://
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<GeminiService>();
builder.Services.AddScoped<OpenAiService>();
builder.Services.AddScoped<AiFactory>();
builder.Services.AddScoped<UsageService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Configure Stripe
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Use CORS before other middleware
app.UseCors("AllowChromeExtension");

// Enable Swagger in all environments for testing
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();