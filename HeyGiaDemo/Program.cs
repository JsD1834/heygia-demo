using HeyGiaDemo.Context;
using HeyGiaDemo.Services;
using HeyGiaDemo.Services.Llm;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HeyGiaDemoDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios
builder.Services.AddSingleton<LeadScoringService>();

var provider = "LocalRule";
if (provider.Equals("HuggingFace", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddHttpClient<HuggingFaceLlmClient>();
    builder.Services.AddTransient<ILlmClient>(sp => sp.GetRequiredService<HuggingFaceLlmClient>());
}
else if (provider.Equals("LocalRule", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddTransient<ILlmClient, LocalRuleLlmClient>();
}

builder.Services.AddScoped<IPromotorService, PromotorService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
