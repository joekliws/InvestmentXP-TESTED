global using AutoMapper;
using Investment.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddServiceScope();

builder.Services.AddRepositoryScope();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseApiConfiguration();

app.Run();
