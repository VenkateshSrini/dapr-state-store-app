using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddDapr()
                .AddJsonOptions(jsonOptions => { 
                    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy
                    = JsonNamingPolicy.CamelCase;
                    jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.MapControllers();

app.Run();
