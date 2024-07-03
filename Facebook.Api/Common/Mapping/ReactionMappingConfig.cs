using Facebook.Application.Reaction.Command.Add;
using Facebook.Application.Reaction.Command.Delete;
using Facebook.Contracts.Reaction.Add;
using Facebook.Contracts.Reaction.Delete;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class ReactionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddReactionRequest, AddReactionCommand>();
            //.Map(dest => dest.PostId, src => src.PostId)
            //.Map(dest => dest.UserId, src => src.UserId)
            //.Map(dest => dest.TypeCode, src => src.TypeCode);

        config.NewConfig<DeleteReactionRequest, DeleteReactionCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.PostId, src => src.PostId);

    }
}
