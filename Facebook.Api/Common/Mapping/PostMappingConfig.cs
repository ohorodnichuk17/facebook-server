using AutoMapper;
using Facebook.Application.Post.Command.Create;
using Facebook.Contracts.Post.Create;

namespace Facebook.Server.Common.Mapping;

public class PostMappingConfig : Profile
{
    public PostMappingConfig()
    {
        CreateMap<CreatePostRequest, CreatePostCommand>();
    }
}