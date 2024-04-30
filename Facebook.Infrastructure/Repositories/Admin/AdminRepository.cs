// using ErrorOr;
// using Facebook.Application.Common.Admin;
// using Facebook.Domain.User;
// using MediatR;
// using Microsoft.AspNetCore.Identity;
//
// namespace Facebook.Infrastructure.Repositories.Admin;
//
// public class AdminRepository : IAdminRepository
// {
//     private readonly UserManager<User> _userManager;
//     
//     public AdminRepository(UserManager<User> userManager)
//     {
//         _userManager = userManager;
//     }
//     
     // public async Task<ErrorOr<User>> CreateAsync(User user, string password, string role)
     // {
     //     user.UserName = $"{user.Email}".ToLower();
     //
     //     var createUserResult = await _userManager.CreateAsync(user, password);
     //
     //     if (!createUserResult.Succeeded)
     //     {
     //         foreach (var error in createUserResult.Errors)
     //         {
     //             Console.WriteLine($"Error creating user: {error.Description}");
     //         }
     //         return Error.Failure("Error creating user");
     //     }
     //
     //     var addToRoleResult = await _userManager.AddToRoleAsync(user, role);
     //
     //     if (!addToRoleResult.Succeeded)
     //     {
     //         foreach (var error in addToRoleResult.Errors)
     //         {
     //             Console.WriteLine($"Error adding user to role: {error.Description}");
     //         }
     //         return Error.Failure("Error adding user to role");
     //     }
     //
     //     return user;
     // }
//
//     public Task<ErrorOr<Unit>> BlockUserAsync(string userId)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<Unit>> UnblockUserAsync(string userId)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<Unit>> SetProfilePrivacyAsync(string userId, bool isProfilePublic)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<bool>> GetProfilePrivacyAsync(string userId)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<Unit>> DeleteUserAsync(string userId)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<User>> GetUserByEmailAsync(string email)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<User>> GetUserByIdAsync(string userId)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<List<User>>> GetAllUsersAsync()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<List<User>>> SearchUsersAsync(string firstName, string lastName)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<List<User>>> GetRecentlyAddedUsersAsync(DateTime sinceDate)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<ErrorOr<List<User>>> GetBlockedUsersAsync()
//     {
//         throw new NotImplementedException();
//     }
// }