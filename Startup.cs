using System.Text;
using Hackathon2024API.Data;
using Hackathon2024API.Data.Settings;
using Hackathon2024API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Hackathon2024API;

public static class Startup
{
    public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var options = builder.Configuration.GetSection(JwtSettings.DefaultSection).Get<JwtSettings>();
            var jwtKey = options.JwtKey;
            var issuer = options.Issuer;
            var audience = options.Audience;
            
            services.AddAuthorization();
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowNgrok",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            
            services.AddIdentity<User, IdentityRole<long>>(identityOptions =>
                {
                    identityOptions.Password.RequiredLength = 5;
    
                })
                .AddRoles<IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        }
    
    public static void AddSwagger(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
            
        services.AddSwaggerGen(swaggerOptions =>
        {
            swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Введите валидный токен",
                Name = "Авторизация",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
                
            swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
                
        });
    }

}