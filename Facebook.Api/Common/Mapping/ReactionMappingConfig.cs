using Facebook.Application.Reaction.Command.Add;
using Facebook.Application.Reaction.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Reaction.Add;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class ReactionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddReactionRequest, AddReactionCommand>();

        config.NewConfig<DeleteRequest, DeleteReactionCommand>()
            .Map(dest => dest.Id, src => src.Id);
    }
}
