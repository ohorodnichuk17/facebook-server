using Facebook.Domain.UserEntity;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Facebook.Application.Common.Interfaces.Services;

public interface IImageStorageService
{
    Task<string?> AddAvatarAsync(UserEntity user, byte[]? file);
    Task<ErrorOr<string>> SaveImageAsync(IFormFile image);
    Task<ErrorOr<Deleted>> DeleteImageAsync(string imageName);
}