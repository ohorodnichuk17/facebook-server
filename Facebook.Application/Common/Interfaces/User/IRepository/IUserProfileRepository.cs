using ErrorOr;
using Facebook.Domain.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Common.Interfaces.Persistance;

public interface IUserProfileRepository
{
    public Task<ErrorOr<UserProfileEntity>> UserEditProfileAsync(UserProfileEntity userProfile,
        string firstName, string lastName, string avatar);
    Task<ErrorOr<UserProfileEntity>> UserCreateProfileAsync(Guid userId);
    Task<ErrorOr<UserProfileEntity>> GetUserProfileByIdAsync(string userId);
    Task<ErrorOr<bool>> DeleteUserProfileAsync(string userId);
    Task<ErrorOr<Unit>> BlockUserAsync(string userId);
    Task<ErrorOr<Unit>> UnblockUserAsync(string userId);
}
