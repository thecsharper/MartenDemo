using Marten;
using MartenDemo;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// TODO add to config
var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

var connection = configuration.GetConnectionString("Marten");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => 
{
    //c.SwaggerDoc("v2", new OpenApiInfo { Title = "Test API", Version = "0.0.1" });
    //c.ResolveConflictingActions(x => x.First());
});

builder.Services.AddMarten(connection);
builder.Services.AddTransient<IMartenQueryBuilder, MartenQueryBuilder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MartenDemo v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
