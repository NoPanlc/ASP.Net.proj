using Microsoft.Extensions.Configuration;
using Weather.Server.DTOs;
using Weather.Server.Interfaces;
using Weather.Server.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<HttpClient>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IUrlBuilderInterface, UrlBuilderService>();
builder.Services.AddSwaggerGen(opt =>
{
    opt.CustomSchemaIds(type => type.FullName);
});
builder.Services.Configure<OpenWeather>(builder.Configuration.GetSection("OpenWeather"));




var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
