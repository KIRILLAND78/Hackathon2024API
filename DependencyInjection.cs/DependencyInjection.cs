using Hackathon2024API.Interfaces.Services;
using Hackathon2024API.Models;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon2024API.DependencyInjection.cs
{
	public static class DependencyInjection
	{
		/*public static void AddApplication(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(ReportMapping));
			services.AddAutoMapper(typeof(AuthorMapping));
			InitServices(services);
		}

		public static void InitServices(this IServiceCollection services)
		{
			services.AddScoped<IReportValidator, ReportValidator>();
			services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
			services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();
			services.AddScoped<IReportService, ArticleService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IAuthorService, AuthorService>();
			services.AddScoped<UserManager<User>>();
		}*/
	}
}
