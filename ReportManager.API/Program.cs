using Microsoft.EntityFrameworkCore;
using ReportManager.Infrastructure.Data;
using ReportManager.Application.Interfaces;
using ReportManager.Infrastructure.Repositories;
using ReportManager.Application.Services;
using ReportManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Existing ---  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Your additions ---  
builder.Services.AddDbContext<NewsDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("ReportManager.Infrastructure")));

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IReporterRepository, ReporterRepository>();

builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IReporterService, ReporterService>();

builder.Services.AddScoped<IPersianDateConverter, PersianDataConverter>();
builder.Services.AddScoped<ICsvImporter, CsvImporter>();

builder.Services.AddScoped<ISourceDataImporter, SourceDataImporter>();

builder.Services.AddControllers();     

var app = builder.Build();

var inputFolder = Path.Combine(builder.Environment.ContentRootPath, "Data", "Input");
using (var scope = app.Services.CreateScope())
{
    var importer = scope.ServiceProvider.GetRequiredService<ISourceDataImporter>();
    await importer.ImportAllAsync(inputFolder);
    Console.WriteLine("Finished importing all source data.");
}


// For Running Unit & Integration Tests
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<NewsDbContext>();
//     // Create schema directly from the model:
//     db.Database.EnsureCreated();
// }


if (app.Environment.IsDevelopment())  
{  
    app.UseSwagger();  
    app.UseSwaggerUI();  
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();