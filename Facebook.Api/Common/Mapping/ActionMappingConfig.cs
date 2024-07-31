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
        config.NewConfig<AddActionRequest, AddActionCommand>();

        config.NewConfig<AddActionCommand, ActionEntity>();

        config.NewConfig<DeleteRequest, DeleteActionCommand>();
    }
}
