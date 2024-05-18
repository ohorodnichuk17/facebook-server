using AutoMapper;
using Facebook.Application.Story.Command.Delete;
using Facebook.Contracts.Story.Delete;

namespace Facebook.Server.Common.Mapping;

public class StoryMappingConfig : Profile
{
    public StoryMappingConfig()
    {
        CreateMap<DeleteStoryRequest, DeleteStoryCommand>();
    }
}