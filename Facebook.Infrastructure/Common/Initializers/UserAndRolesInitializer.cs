using Facebook.Domain.Constants.Roles;
using Facebook.Domain.Post;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Facebook.Infrastructure.Common.Initializers;

public static class UserAndRolesInitializer
{
    public static async void SeedData(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var service = scope.ServiceProvider;

            var context = service.GetRequiredService<FacebookDbContext>();
            await context.Database.MigrateAsync();

            var userManager = service.GetRequiredService<UserManager<UserEntity>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            if (!context.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = Roles.Admin });
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = Roles.User });
            }

            if (!context.Users.Any())
            {
                var user = new UserEntity
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "isgrassisgreen@gmail.com",
                    UserName = "isgrassisgreen@gmail.com",
                    EmailConfirmed = true,
                    Birthday = DateTime.Today,
                    Gender = "Male",
                };

                var result = await userManager.CreateAsync(user, "bAazxcQ94@?");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }

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

            if (!context.Actions.Any())
            {
                var actions = new List<ActionEntity>
                {
                    new()
                    {
                         Name="Celebrating",
                         Emoji="Celebrating",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="New Year" },
                             new() { Name="Birthday" },
                             new() { Name="Love" },
                             new() { Name="Christmas" },
                             new() { Name="Friday" },
                         }
                    },
                    new()
                    {
                         Name="Drinking",
                         Emoji="Drinking",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Water" },
                             new() { Name="Juice" },
                             new() { Name="Wine" },
                             new() { Name="Beer" },
                         }
                    },
                    new()
                    {
                         Name="Eating",
                         Emoji="Eating",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Dinner" },
                             new() { Name="Breakfast" },
                             new() { Name="Lunch" },
                         }
                    },
                    new()
                    {
                         Name="Flying to",
                         Emoji="Flying to",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="USA" },
                             new() { Name="France" },
                             new() { Name="Portugal" },
                             new() { Name="Spain" },
                         }
                    },
                    new()
                    {
                         Name="Hearing",
                         Emoji="Hearing",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Music" },
                         }
                    },
                    new()
                    {
                         Name="Participating in",
                         Emoji="Participating in",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Wedding" },
                             new() { Name="Celebrating christmas" },
                             new() { Name="Birthday organization" },
                             new() { Name="Football match" },
                         }
                    },
                    new()
                    {
                         Name="Playing",
                         Emoji="Playing",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Xbox" },
                             new() { Name="Playstation" },
                             new() { Name="Basketball" },
                             new() { Name="Football" },
                         }
                    },
                    new()
                    {
                         Name="Reading",
                         Emoji="Reading",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Romance novel" },
                             new() { Name="Twilight" },
                             new() { Name="The Da Vinci Code" },
                             new() { Name="Harry Potter" },
                         }
                    },
                    new()
                    {
                         Name="Searching",
                         Emoji="Searching",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Balance" },
                             new() { Name="Answers" },
                             new() { Name="Perfection" },
                         }
                    },
                    new()
                    {
                         Name="Viewing",
                         Emoji="Viewing",
                         SubActions=new List<SubActionEntity>
                         {
                             new() { Name="Rick and Morty" },
                             new() { Name="Our planet" },
                             new() { Name="News" },
                         }
                    },
                };

                context.Actions.AddRange(actions);

                await context.SaveChangesAsync();
            }
        }
    }
}