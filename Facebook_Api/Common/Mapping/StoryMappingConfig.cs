using Facebook.Application.DTO;
using Facebook.Application.Story.Command.Create;
using Facebook.Contracts.Story.Create;
using Facebook.Domain.Story;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class StoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateStoryRequest request, byte[] Image), CreateStoryCommand>()
            .Map(dest => dest.Content, src => src.request.Content)
            .Map(dest => dest.Image, src => src.Image);

        config.NewConfig<CreateStoryCommand, StoryEntity>()
            .Map(dest => dest.CreatedAt, src => DateTime.Now)
            .Ignore(nameof(CreateStoryRequest.Image));

        config.NewConfig<StoryEntity, StoryEntity>()
                .Map(dest => dest.User, src => src.User.Adapt<UserForPostDto>())
                .PreserveReference(true);
    }
}