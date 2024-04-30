using ErrorOr;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Http;

namespace Facebook.Application.Common.Interfaces.Services;

public interface IImageStorageService
{
    Task<string?> AddAvatarAsync(UserEntity user, byte[]? file);
    Task<string?> AddPostImageAsync(byte[]? file);
    Task<string?> AddStoryImageAsync(byte[]? file);
    Task<bool> DeleteImageAsync(string imageName);
}