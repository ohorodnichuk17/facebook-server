using Facebook.Application.Story.Command.Create;
using Facebook.Contracts.Story.Create;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class StoryMappingConfig
{
    public StoryMappingConfig(TypeAdapterConfig config)
    {
        config.NewConfig<CreateStoryRequest, CreateStoryCommand>();
    }
}