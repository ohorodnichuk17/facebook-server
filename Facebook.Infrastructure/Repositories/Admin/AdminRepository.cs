using ErrorOr;
using Facebook.Application.Common.Interfaces.Admin;
using Facebook.Application.Common.Interfaces.Admin.IRepository;
using Facebook.Domain.User;
using Facebook.Infrastructure.Common.Persistence;
using Facebook.Infrastructure.Repositories.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Infrastructure.Repositories.Admin;

public class AdminRepository(UserManager<UserEntity> userManager, FacebookDbContext context) 
    : UserRepository(userManager, context), IAdminRepository
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<ErrorOr<UserEntity>> CreateAsync(UserEntity user, string password, string role)
     {
         user.UserName = $"{user.Email}".ToLower();
     
         var createUserResult = await _userManager.CreateAsync(user, password);
     
         if (!createUserResult.Succeeded)
         {
             foreach (var error in createUserResult.Errors)
             {
                 Console.WriteLine($"Error creating user: {error.Description}");
             }
             return Error.Failure("Error creating user");
         }
     
         var addToRoleResult = await _userManager.AddToRoleAsync(user, role);
     
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

    public async Task<ErrorOr<Unit>> DeleteUserAsync(string userId)
    {
        try
        {
            var userToDelete = await _userManager.FindByIdAsync(userId);
            if (userToDelete == null)
            {
                return Error.Failure("User not found");
            }

            var deleteResult = await _userManager.DeleteAsync(userToDelete);
            if (!deleteResult.Succeeded)
            {
                foreach (var error in deleteResult.Errors)
                {
                    Console.WriteLine($"Error deleting user: {error.Description}");
                }
                return Error.Failure("Error deleting user");
            }

            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Error.Failure("An error occurred while deleting the user");
        }
    }

    public async Task<ErrorOr<UserEntity>> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
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

    public new async Task<ErrorOr<UserEntity>> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
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

    public new async Task<ErrorOr<List<UserEntity>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("An error occurred while retrieving the users");
        }
    }
}