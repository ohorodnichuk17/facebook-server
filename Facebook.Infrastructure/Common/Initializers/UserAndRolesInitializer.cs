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
    public async static void SeedData(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var service = scope.ServiceProvider;

            var context = service.GetRequiredService<FacebookDbContext>();

            context.Database.Migrate();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<UserEntity>>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

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
                    Email = "admin@gmail.com",
                    // PasswordHash = "Admin123*",
                    EmailConfirmed = true,
                    Birthday = DateTime.Today,
                    Gender = "Male",
                };
                var result = userManager.CreateAsync(user, "Admin123*").Result;
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }

            if (!context.Feelings.Any())
            {
                var feelings = new List<FeelingEntity>() {
                    new()
                    {
                         Name="Angry",
                         Emoji="Angry"
                    },new()
                    {
                         Name="Happy",
                         Emoji="Happy"
                    },new()
                    {
                         Name="In Love",
                         Emoji="In Love"
                    },new()
                    {
                         Name="Laughing",
                         Emoji="Laughing"
                    },new()
                    {
                         Name="Sad",
                         Emoji="Sad"
                    },new()
                    {
                         Name="Shocked",
                         Emoji="Shocked"
                    },new()
                    {
                         Name="Sick",
                         Emoji="Sick"
                    },new()
                    {
                         Name="Smiling",
                         Emoji="Smiling"
                    },new()
                    {
                         Name="Starstruck",
                         Emoji="Starstruck"
                    },new()
                    {
                         Name="Suprized",
                         Emoji="Suprized"
                    },new()
                    {
                         Name="Wink",
                         Emoji="Wink"
                    },
                };

                context.Feelings.AddRange(feelings);
                await context.SaveChangesAsync();
            }
        }
    }
}
