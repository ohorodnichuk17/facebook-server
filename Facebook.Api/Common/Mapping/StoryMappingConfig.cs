using AutoMapper;
using Facebook.Application.Story.Command.Create;
using Facebook.Contracts.Story.Create;

namespace Facebook.Server.Common.Mapping;

public class StoryMappingProfile : Profile
{
    public StoryMappingProfile()
    {
        CreateMap<CreateStoryRequest, CreateStoryCommand>();
    }
}