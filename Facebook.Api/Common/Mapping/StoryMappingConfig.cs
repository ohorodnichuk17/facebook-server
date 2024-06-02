using Facebook.Application.Story.Command.Create;
using Facebook.Contracts.Story.Create;
using Facebook.Domain.Story;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class StoryMappingConfig 
{
    public StoryMappingConfig()
    {
        config.NewConfig<(CreateStoryRequest request, string BaseUrl, byte[] Image), CreateStoryCommand>()
            .Map(dest => dest.Image, src => src.Image)
            .Map(dest => dest.UserId, src => src.request.UserId)
            .Map(dest => dest, src => src.request);
    }
}