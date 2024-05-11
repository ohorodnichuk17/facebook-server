using AutoMapper;
using Facebook.Application.Story.Command.Create;
using Facebook.Contracts.Story.Create;

namespace Facebook.Application.Helpers;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CreateStoryRequest, CreateStoryCommand>();
    }
}
