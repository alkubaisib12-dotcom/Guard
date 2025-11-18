using Azure.Core;
using Azure.Identity;
using GuardDBcallsAPI.Controllers;
using GuardDBcallsAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// DATABASE SETUP
// --------------------
builder.Services.AddDbContext<GuardSystemContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});


// --------------------
// SERVICES
// --------------------
// User & Auth
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<JwtService>();

// Rooms & Devices
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISmartPlugService, SmartPlugService>();
builder.Services.AddScoped<IIrblasterService, IrblasterService>();



// Hazards & Recommendations
builder.Services.AddScoped<IHazardLogService, HazardLogService>();
builder.Services.AddScoped<IRecommendationsService, RecommendationsService>();

// Dashboard & Setup
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ISystemSetupService, SystemSetupService>();

builder.Services.AddControllers();

// --------------------
// CORS
// --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    policy.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

// --------------------
// JSON OPTIONS
// --------------------
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

var app = builder.Build();

// --------------------
// MIDDLEWARE
// --------------------
app.UseHttpsRedirection();
app.UseCors("AllowAll");
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
