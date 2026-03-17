using Server.Services;
using Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register MongoDB service
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();

// Register Trading service
builder.Services.AddScoped<ITradingService, TradingService>();

// Register Quest service
builder.Services.AddScoped<IQuestService, QuestService>();

// Register Obstacle service
builder.Services.AddScoped<IObstacleService, ObstacleService>();

// Register Crafting service
builder.Services.AddScoped<ICraftingService, CraftingService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();

