using UnmasqueradeApi.Core;
using UnmasqueradeApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UnmasqueradeDatabaseSettings>(builder.Configuration.GetSection("Database"));
builder.Services.Configure<TmdbSettings>(builder.Configuration.GetSection("TMDB"));
builder.Services.AddSingleton<MoviesService>();
builder.Services.AddSingleton<TMDBService>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();