using Bogus;
using Facebook.Domain.Constants.Roles;
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
                var admin = new UserEntity
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "isgrassisgreen@gmail.com",
                    UserName = "isgrassisgreen@gmail.com",
                    EmailConfirmed = true,
                    Birthday = DateTime.Today,
                    Gender = "Female",
                };

                var result = await userManager.CreateAsync(admin, "bAazxcQ94@?");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }

                var faker = new Faker<UserEntity>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                    .RuleFor(u => u.UserName, (f, u) => u.Email)
                    .RuleFor(u => u.EmailConfirmed, f => true)
                    .RuleFor(u => u.Birthday, f => f.Date.Past(30, DateTime.Today.AddYears(-18)))
                    .RuleFor(u => u.Gender, f => f.PickRandom("Male", "Female"));

                for (int i = 0; i < 50; i++)
                {
                    var user = faker.Generate();
                    var userResult = await userManager.CreateAsync(user, "User@1234");

                    if (userResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, Roles.User);

                        var userProfile = new Faker<UserProfileEntity>()
                            .RuleFor(p => p.Biography, f => f.Lorem.Sentence())
                            .RuleFor(p => p.Country, f => f.Address.Country())
                            .RuleFor(p => p.Region, f => f.Address.State())
                            .RuleFor(p => p.Pronouns, f => f.PickRandom(new[] { "he/him", "she/her", "they/them", "do not specify" }))
                            .RuleFor(p => p.IsProfilePublic, f => f.Random.Bool())
                            .Generate();

                        userProfile.UserEntity = user;
                        userProfile.UserId = user.Id;

                        await context.UsersProfiles.AddAsync(userProfile);
                    }
                }
            }
        }
    }
}