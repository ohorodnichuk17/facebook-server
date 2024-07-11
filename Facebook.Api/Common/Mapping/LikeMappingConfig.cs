using Facebook.Application.Like.Command.Add;
using Facebook.Application.Like.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Like.Create;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class LikeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddLikeRequest, AddLikeCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.PostId, src => src.PostId);

        config.NewConfig<DeleteRequest, DeleteLikeCommand>()
            .Map(dest => dest.Id, src => src.Id);
    }
}
