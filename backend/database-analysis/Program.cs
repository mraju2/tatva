using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using database_analysis.Interfaces;
using database_analysis.services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(configure => configure.AddConsole())
        .AddTransient<IDatabaseConnectionService, DatabaseConnectionService>()
        .AddTransient<IDatabaseExtractionService, DatabaseExtractionService>()
        .AddTransient<IFileStorageService, FileStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
