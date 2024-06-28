using Facebook.Application.Story.Command.Create;
using Facebook.Contracts.Story.Create;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class StoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateStoryRequest request, byte[] Image), CreateStoryCommand>()
            .Map(dest => dest.Content, src => src.request.Content)
            .Map(dest => dest.Image, src => src.Image)
            .Map(dest => dest.UserId, src => src.request.UserId);
    }
}