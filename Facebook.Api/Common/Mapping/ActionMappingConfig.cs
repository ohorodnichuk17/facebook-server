using Facebook.Application.Action.Command.Add;
using Facebook.Application.Action.Command.Delete;
using Facebook.Contracts.Action.Add;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Post;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class ActionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddActionRequest, AddActionCommand>()
           .Map(dest => dest.Name, src => src.Name)
           .Map(dest => dest.Emoji, src => src.Emoji);

        config.NewConfig<DeleteRequest, DeleteActionCommand>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<AddActionCommand, ActionEntity>();
    }
}
