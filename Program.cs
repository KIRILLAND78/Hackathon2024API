using Hackathon2024API;
using Hackathon2024API.Data;
using Hackathon2024API.Data.Settings;
using Hackathon2024API.Interfaces.Services;
using Hackathon2024API.Models;
using Hackathon2024API.Repository;
using Hackathon2024API.Services;
using log4net.Config;
using Hackathon2024API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddScoped<IAuthService, AuthSrevice>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<EncryptionService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech Swagger v 1.0");
        c.RoutePrefix = "swagger";
    });
}

XmlConfigurator.Configure(new FileInfo("log4net.config"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowNgrok");



app.Run();
