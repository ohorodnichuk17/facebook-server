using System.Net.Mime;
using ErrorOr;
using Facebook.Application.Common.Interfaces.Services;
using Facebook.Domain.UserEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Facebook.Infrastructure.Services.Common;

public class ImageStorageService : IImageStorageService
{
    public async Task<string> AddAvatarAsync(UserEntity user, byte[] file)
    {
        string imageName = Path.GetRandomFileName() + ".webp";
        var uploadFolderPath = Path.Combine(
            Directory.GetCurrentDirectory(), "images", "avatars");

        if (!Directory.Exists(uploadFolderPath))
        {
            Directory.CreateDirectory(uploadFolderPath);
        }

        if (!user.Avatar.IsNullOrEmpty())
        {
            var deleteFilePath = Path.Combine(
                uploadFolderPath, user.Avatar!);
            if (File.Exists(deleteFilePath))
            {
                File.Delete(deleteFilePath);
            }
        }

        string dirSaveImage = Path.Combine(uploadFolderPath, imageName);
        using var image = Image.Load(file);
        image.Mutate(x =>
        {
            x.Resize(new ResizeOptions
            {
                Size = new Size(1200, 1200),
                Mode = ResizeMode.Max
            });
        });

        using var stream = File.Create(dirSaveImage);
        await image.SaveAsync(stream, new WebpEncoder());

        return imageName;
    }
    

    public Task<ErrorOr<string>> SaveImageAsync(IFormFile image)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Deleted>> DeleteImageAsync(string imageName)
    {
        throw new NotImplementedException();
    }
}