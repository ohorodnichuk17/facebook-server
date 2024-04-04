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
	public async static void SeedData(this IApplicationBuilder app)
	{
		using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
		{
			var service = scope.ServiceProvider;

			var context = service.GetRequiredService<FacebookDbContext>();

			context.Database.Migrate();

			var userManager = scope.ServiceProvider
				.GetRequiredService<UserManager<User>>();

			var roleManager = scope.ServiceProvider
				.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			if (!context.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole<Guid> { Name = Roles.Admin });
				await roleManager.CreateAsync(new IdentityRole<Guid> { Name = Roles.User });
			}

			if (!context.Users.Any())
			{
				var user = new User
				{
					FirstName = "Admin",
					LastName = "Admin",
					Email = "admin@gmail.com",
					PasswordHash = "Admin123*",
					EmailConfirmed = true,
					Birthday = DateTime.Today,
					Gender = "Male",
					IsBlocked = false
				};
				var result = userManager.CreateAsync(user, "Admin123*").Result;
				if (result.Succeeded)
				{
					result = userManager.AddToRoleAsync(user, Roles.Admin).Result;
				}
			}


		}
	}
}
