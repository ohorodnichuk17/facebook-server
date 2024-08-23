using Bogus;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Facebook.Infrastructure.Common.Initializers
{
    public static class UserAndRolesInitializer
    {
        public static async void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                Random random = new Random();

                string basePath = Directory.GetCurrentDirectory();
                string avatarsPath = Path.Combine(basePath, "images", "avatars");
                string coverPhotosPath = Path.Combine(basePath, "images", "coverPhotos");
                string storiesPath = Path.Combine(basePath, "images", "stories");
                string[] avatars = Directory.GetFiles(avatarsPath);
                string[] stories = Directory.GetFiles(storiesPath);
                string[] coverPhotos = Directory.GetFiles(coverPhotosPath);

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
                    var adminUser = new UserEntity
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "isgrassisgreen@gmail.com",
                        UserName = "isgrassisgreen@gmail.com",
                        EmailConfirmed = true,
                        Birthday = DateTime.Today,
                        Gender = "Male",
                    };

                    var adminResult = await userManager.CreateAsync(adminUser, "bAazxcQ94@?");

                    if (adminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                    }

                    var faker = new Faker<UserEntity>()
                        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                        .RuleFor(u => u.LastName, f => f.Name.LastName())
                        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                        .RuleFor(u => u.UserName, (f, u) => u.Email)
                        .RuleFor(u => u.EmailConfirmed, f => true)
                        .RuleFor(u => u.Birthday, f => f.Date.Past(30, DateTime.Today.AddYears(-18)))
                        .RuleFor(u => u.Gender, f => f.PickRandom("Male", "Female"));

                    var users = new List<UserEntity>();
                    int privateProfileCount = 0; 

                    for (int i = 0; i < 50; i++)
                    {
                        var newUser = faker.Generate();
                        newUser.Avatar = Path.GetFileName(avatars[random.Next(avatars.Length)]);
                        var userResult = await userManager.CreateAsync(newUser, "User@1234");

                        if (userResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newUser, Roles.User);

                            var isProfilePublic = privateProfileCount >= 3 || new Faker().Random.Bool();
                            if (!isProfilePublic)
                            {
                                privateProfileCount++; 
                            }

                            var newUserProfile = new Faker<UserProfileEntity>()
                                .RuleFor(p => p.Biography, f => f.Lorem.Sentence())
                                .RuleFor(p => p.Country, f => f.Address.Country())
                                .RuleFor(p => p.Region, f => f.Address.State())
                                .RuleFor(p => p.Pronouns, f => f.PickRandom(new[] { "he/him", "she/her", "they/them" }))
                                .RuleFor(p => p.IsProfilePublic, f => isProfilePublic)
                                .Generate();

                            newUserProfile.UserEntity = newUser;
                            newUserProfile.UserId = newUser.Id;
                            newUserProfile.CoverPhoto = Path.GetFileName(coverPhotos[random.Next(coverPhotos.Length)]);

                            context.UsersProfiles.Add(newUserProfile);
                            users.Add(newUser);
                        }
                    }

                    await context.SaveChangesAsync();

                    var friendRequestFaker = new Faker<FriendRequestEntity>()
                        .RuleFor(fr => fr.SenderId, f => f.PickRandom(users).Id)
                        .RuleFor(fr => fr.ReceiverId, f => f.PickRandom(users).Id)
                        .RuleFor(fr => fr.SentAt, f => f.Date.Recent(10))
                        .RuleFor(fr => fr.IsAccepted, f => f.Random.Bool());

                    for (int i = 0; i < 20; i++)
                    {
                        var friendRequest = friendRequestFaker.Generate();

                        if (friendRequest.SenderId != friendRequest.ReceiverId)
                        {
                            context.FriendRequests.Add(friendRequest);
                        }
                    }

                    var storyFaker = new Faker<StoryEntity>()
                        .RuleFor(s => s.Content, f => f.Lorem.Paragraph())
                        .RuleFor(s => s.Image, f => f.Image.PicsumUrl())
                        .RuleFor(s => s.CreatedAt, f => f.Date.Recent(1))
                        .RuleFor(s => s.UserId, f => f.PickRandom(users).Id);

                    for (int i = 0; i < 50; i++)
                    {
                        var story = storyFaker.Generate();
                        story.Image = Path.GetFileName(stories[random.Next(stories.Length)]);
                        context.Stories.Add(story);
                    }

                    var postFaker = new Faker<PostEntity>()
                        .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                        .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                        .RuleFor(p => p.Location, f => f.Address.City())
                        .RuleFor(p => p.Tags, f => f.Lorem.Words(3).ToList())
                        .RuleFor(p => p.CreatedAt, f => f.Date.Recent(2))
                        .RuleFor(p => p.Visibility, f => f.PickRandom(new[] { "public", "private", "friends only", "friends except" }))
                        .RuleFor(p => p.UserId, f => f.PickRandom(users).Id);

                    for (int i = 0; i < 50; i++)
                    {
                        var post = postFaker.Generate();
                        context.Posts.Add(post);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
