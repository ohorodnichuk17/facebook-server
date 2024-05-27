using System.Reflection;
using Facebook.Application.Common.Behaviours;
using Facebook.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Facebook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssemblies(AppDomain.CurrentDomain
                .GetAssemblies()));

        services.AddScoped(typeof(IPipelineBehavior<,>), 
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<EmailService>();

        return services;
    }
}