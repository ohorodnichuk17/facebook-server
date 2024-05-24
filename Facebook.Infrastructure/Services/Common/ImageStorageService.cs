using Facebook.Application.Common.Interfaces.Services;
using Facebook.Domain.User;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Facebook.Infrastructure.Services.Common;

public class ImageStorageService : IImageStorageService
{
    public async Task<string?> AddAvatarAsync(UserEntity user, byte[]? file)
    {
        if (file == null)
        {
            return null;
        }
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
    public async Task<string?> AddPostImageAsync(byte[]? file)
    {
        if (file == null)
        {
            return null;
        }

        string imageName = Path.GetRandomFileName() + ".webp";
        var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "posts");

        if (!Directory.Exists(uploadFolderPath))
        {
            Directory.CreateDirectory(uploadFolderPath);
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
    
    public async Task<string?> AddStoryImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "stories");

        try
        {
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            string filePath = Path.Combine(uploadFolderPath, imageName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return imageName;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while saving the image: {ex.Message}");
            return null;
        }
    }


    public async Task<bool> DeleteImageAsync(string imageName)
    {
        try
        {
            var postFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "posts");
            var storyFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "stories");

            var postFilePath = Path.Combine(postFolderPath, imageName);
            if (File.Exists(postFilePath))
            {
                File.Delete(postFilePath);
                return true;
            }

            var storyFilePath = Path.Combine(storyFolderPath, imageName);
            if (File.Exists(storyFilePath))
            {
                File.Delete(storyFilePath);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error with deleting image: {ex.Message}");
            return false;
        }
    }
}