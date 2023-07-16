using ElasticSearch.API.Nest.Extensions;
using ElasticSearch.API.Nest.Repositories;
using ElasticSearch.API.Nest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomElasticSearch(builder.Configuration);
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();
