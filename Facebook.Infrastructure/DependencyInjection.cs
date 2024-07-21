using Facebook.Application.Common.Interfaces.Admin.IRepository;
using Facebook.Application.Common.Interfaces.Admin.IService;
using Facebook.Application.Common.Interfaces.Authentication;
using Facebook.Application.Common.Interfaces.Chat.IRepository;
using Facebook.Application.Common.Interfaces.Comment.IRepository;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.Common.Interfaces.Like.IRepository;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Application.Common.Interfaces.Story.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.User;
using Facebook.Infrastructure.Authentication;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Infrastructure.Repositories.Admin;
using Facebook.Infrastructure.Repositories.Chat;
using Facebook.Infrastructure.Repositories.Comment;
using Facebook.Infrastructure.Repositories.Feeling;
using Facebook.Infrastructure.Repositories.Like;
using Facebook.Infrastructure.Repositories.Post;
using Facebook.Infrastructure.Repositories.Reaction;
using Facebook.Infrastructure.Repositories.Story;
using Facebook.Infrastructure.Repositories.User;
using Facebook.Infrastructure.Services.Admin;
using Facebook.Infrastructure.Services.Common;
using Facebook.Infrastructure.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
        services.AddIdentity<UserEntity, IdentityRole<Guid>>(option =>
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
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IStoryRepository, StoryRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IFeelingRepository, FeelingRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IStoryRepository, StoryRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IReactionRepository, ReactionRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IFeelingRepository, FeelingRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtGenerator, JwtGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<ISmtpService, SmtpService>();
        services.AddTransient<SmtpService>();

        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddTransient<UserAuthenticationService>();

        services.AddScoped<IImageStorageService, ImageStorageService>();
        services.AddTransient<ImageStorageService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<CurrentUserService>();

        return services;
    }

    private static IServiceCollection AddAuth(
       this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtGenerator, JwtGenerator>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole(Roles.Admin));
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}