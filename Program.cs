using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Medals.Models;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(configuration["ConnectionStrings:DefaultSQLiteConnection"]));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Open",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Medals API", 
        Version = "v1",
        Description = "Olympic Medals API",
    });
    c.EnableAnnotations();
    c.TagActionsBy(api => new[] { api.HttpMethod });
});

var app = builder.Build();
app.UseCors("Open");

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();