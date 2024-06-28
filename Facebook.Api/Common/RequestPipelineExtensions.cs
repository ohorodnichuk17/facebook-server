using Microsoft.Extensions.FileProviders;

namespace Facebook.Server.Common;

public static class RequestPipelineExtensions
{
    public static void UseCustomStaticFiles(this WebApplication application)
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        application.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(directory),
            RequestPath = "/images"
        });
    }
}