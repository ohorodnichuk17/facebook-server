using Facebook.Domain.User;
using Microsoft.AspNetCore.Http;

namespace Facebook.Application.Common.Interfaces.Common;

public interface IImageStorageService
{
    Task<string?> AddAvatarAsync(UserEntity user, byte[]? file);
    Task<List<string>?> AddPostImagesAsync(List<byte[]> files);
    Task<string?> AddStoryImageAsync(IFormFile? file);
    Task<string?> CoverPhotoAsync(UserProfileEntity userProfile, byte[]? file);
    Task<string?> SaveImageAsByteArrayAsync(byte[] imageBytes);
    Task<bool> DeleteImageAsync(string imageName);
}