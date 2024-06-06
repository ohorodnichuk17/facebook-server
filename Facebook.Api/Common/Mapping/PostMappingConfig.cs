using Facebook.Application.Post.Command.Create;
using Facebook.Contracts.Post.Create;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class PostMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePostRequest, CreatePostCommand>()
            .Map(dest => dest.Images,
                 src => src.Images != null
                        ? src.Images.Select((file, index) => new ImageWithPriority
                        {
                            Image = file.OpenReadStream().ReadAllBytes(),
                            Priority = index
                        }).ToList()
                        : new List<ImageWithPriority>());


    }
}
public static class StreamExtensions
{
    public static byte[] ReadAllBytes(this System.IO.Stream input)
    {
        using (var ms = new System.IO.MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
public class ImageWithPriority
{
    public byte[] Image { get; set; }
    public int Priority { get; set; }
}
