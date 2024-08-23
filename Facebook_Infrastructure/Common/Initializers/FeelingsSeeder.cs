using Facebook.Domain.Post;
using Facebook.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Facebook.Infrastructure.Common.Initializers;

public static class FeelingsSeeder
{
    public static async void SeedData(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var service = scope.ServiceProvider;

            var context = service.GetRequiredService<FacebookDbContext>();


            if (!context.Feelings.Any())
            {
                var feelings = new List<FeelingEntity>
                {
                    new() { Name = "Angry", Emoji = "Angry" },
                    new() { Name = "Happy", Emoji = "Happy" },
                    new() { Name = "In Love", Emoji = "In Love" },
                    new() { Name = "Laughing", Emoji = "Laughing" },
                    new() { Name = "Sad", Emoji = "Sad" },
                    new() { Name = "Shocked", Emoji = "Shocked" },
                    new() { Name = "Sick", Emoji = "Sick" },
                    new() { Name = "Smiling", Emoji = "Smiling" },
                    new() { Name = "Starstruck", Emoji = "Starstruck" },
                    new() { Name = "Suprized", Emoji = "Suprized" },
                    new() { Name = "Wink", Emoji = "Wink" },
                };

                context.Feelings.AddRange(feelings);
                await context.SaveChangesAsync();
            }
        }
    }
}
