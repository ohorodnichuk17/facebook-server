using Facebook.Application.Like.Command.Add;
using Facebook.Application.Like.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Like.Add;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class LikeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LikeRequest, AddLikeCommand>();

        config.NewConfig<LikeRequest, DeleteLikeByPostIdCommand>();

        config.NewConfig<DeleteRequest, DeleteLikeCommand>()
            .Map(dest => dest.Id, src => src.Id);
    }
}
