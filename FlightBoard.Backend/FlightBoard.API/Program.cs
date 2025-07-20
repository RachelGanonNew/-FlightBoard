using FlightBoard.Infrastructure.Persistence;
using FlightBoard.Application.Interfaces;
using FlightBoard.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using FlightBoard.Application.Validators;
using FlightBoard.Application.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register FluentValidation for DTOs
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<FlightCreateValidator>();

// Register FlightStatusService
builder.Services.AddScoped<IFlightStatusService, FlightStatusService>();

// Register FlightService
builder.Services.AddScoped<IFlightService, FlightBoard.Application.Services.FlightService>();

// Register AutoMapper in DI, scanning the Application assembly for profiles.
builder.Services.AddAutoMapper(typeof(FlightBoard.Application.Mapping.FlightProfile).Assembly);

// Register DbContext with SQLite
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<FlightBoard.API.Hubs.FlightHub>("/flightHub");

app.Run();
