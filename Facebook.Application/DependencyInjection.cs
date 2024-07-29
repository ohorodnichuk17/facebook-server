using System.Reflection;
using Facebook.Application.Authentication.SendConfirmationEmail;
using Facebook.Application.Common.Behaviours;
using Facebook.Application.Post.Query.SearchPostsByTags;
using Facebook.Application.Services;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Facebook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMappings();

        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssemblies(AppDomain.CurrentDomain
                .GetAssemblies()));
        
        services.AddScoped(typeof(IPipelineBehavior<,>), 
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<EmailService>();

        return services;
    }
    
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);

        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}