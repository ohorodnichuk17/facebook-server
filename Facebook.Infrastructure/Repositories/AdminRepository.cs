using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository.Admin;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Facebook.Infrastructure.Repositories;

public class AdminRepository(UserManager<UserEntity> userManager, FacebookDbContext context)
    : UserRepository(userManager, context), IAdminRepository
{
    public async Task<ErrorOr<UserEntity>> CreateAsync(UserEntity user, string password, string role)
    {
        user.UserName = $"{user.Email}".ToLower();

        var createUserResult = await userManager.CreateAsync(user, password);

        if (!createUserResult.Succeeded)
        {
            foreach (var error in createUserResult.Errors)
            {
                Console.WriteLine($"Error creating user: {error.Description}");
            }
            return Error.Failure("Error creating user");
        }

        var addToRoleResult = await userManager.AddToRoleAsync(user, role);

        if (!addToRoleResult.Succeeded)
        {
            foreach (var error in addToRoleResult.Errors)
            {
                Console.WriteLine($"Error adding user to role: {error.Description}");
            }
            return Error.Failure("Error adding user to role");
        }

        return user;
    }

    public async Task<ErrorOr<UserEntity>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Error.Failure("User not found");
            }
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while retrieving the user");
        }
    }

    public async Task<ErrorOr<Unit>> BlockUserAsync(string userId)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error.Failure("User not found");
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Error.Failure("Error blocking user");
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while blocking the user");
        }
    }

    public async Task<ErrorOr<Unit>> UnblockUserAsync(string userId)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error.Failure("User not found");
            }

            user.LockoutEnd = null;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Error.Failure("Error unblocking user");
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while unblocking the user");
        }
    }

    public async Task<ErrorOr<Unit>> BanUserAsync(string userId)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error.Failure("User not found");
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;
            user.EmailConfirmed = false;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Error.Failure("Error banning user");
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while banning the user");
        }
    }

    public async Task<ErrorOr<Unit>> UnbanUserAsync(string userId)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error.Failure("User not found");
            }

            user.LockoutEnd = null;
            user.EmailConfirmed = true;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Error.Failure("Error unbanning user");
            }

            return Unit.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while unbanning the user");
        }
    }

    public async Task<ErrorOr<Unit>> RemovePostAsync(string postId)
    {
        try
        {
            var post = await context.Posts.FindAsync(postId);
            if (post == null)
            {
                return Error.Failure("Post not found");
            }

            context.Posts.Remove(post);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while removing the post");
        }
    }

    public async Task<ErrorOr<Unit>> RemoveCommentAsync(string commentId)
    {
        try
        {
            var comment = await context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return Error.Failure("Comment not found");
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while removing the comment");
        }
    }

    public async Task<ErrorOr<Unit>> RemoveStoryAsync(string storyId)
    {
        try
        {
            var story = await context.Stories.FindAsync(storyId);
            if (story == null)
            {
                return Error.Failure("Story not found");
            }

            context.Stories.Remove(story);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while removing the story");
        }
    }
}