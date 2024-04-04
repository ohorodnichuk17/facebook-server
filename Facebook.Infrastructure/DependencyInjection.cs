using System.Text;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Application.Common.Interfaces.Services;
using Facebook.Domain.User;
using Facebook.Infrastructure.Authentication;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Infrastructure.Persistance;
using Facebook.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace Facebook.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		ConfigurationManager configuration)
	{
		services
			.AddPersistence(configuration)
			.AddAppIdentity()
			.AddRepositories()
			.AddInfrastructureServices()
			.AddAuth(configuration);

		return services;
	}

	private static IServiceCollection AddPersistence(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		string connStr = configuration.GetConnectionString("DefaultConnection")!;

		services.AddDbContext<FacebookDbContext>(opt =>
		{
			opt.UseNpgsql(connStr);
		
			opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		});

		return services;
	}

	private static IServiceCollection AddAppIdentity(this IServiceCollection services)
	{
		services.AddIdentity<User, IdentityRole<Guid>>(option =>
		{
			option.SignIn.RequireConfirmedEmail = true;
			option.Lockout.MaxFailedAccessAttempts = 5;
			option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			option.Password.RequireDigit = true;
			option.Password.RequireLowercase = true;
			option.Password.RequireUppercase = true;
			option.Password.RequiredLength = 6;
			option.Password.RequireNonAlphanumeric = true;
			option.User.RequireUniqueEmail = true;
		})
		.AddEntityFrameworkStores<FacebookDbContext>() 
		.AddDefaultTokenProviders();
		

		return services;
	}

	private static IServiceCollection AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUserRepository, UserRepository>();

		return services;
	}

	private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
	{
		services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

		services.AddScoped<ISmtpService, SmtpService>();
		services.AddTransient<SmtpService>();

		services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
		services.AddTransient<UserAuthenticationService>();

		// services.AddScoped<IImageStorageService, ImageStorageService>();
		// services.AddTransient<ImageStorageService>();

		return services;
	}

	private static IServiceCollection AddAuth(
		this IServiceCollection services, ConfigurationManager configuration)
	{
		var jwtSettings = new JwtSettings();
		configuration.Bind(JwtSettings.SectionName, jwtSettings);

		services.AddSingleton(Options.Create(jwtSettings));
		services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

		services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtSettings.Issuer,
				ValidAudience = jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(jwtSettings.Secret)),
				ClockSkew = TimeSpan.Zero,
			});

		return services;
	}
}