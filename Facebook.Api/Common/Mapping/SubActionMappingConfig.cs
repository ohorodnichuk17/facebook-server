using Facebook.Application.SubAction.Command.Add;
using Facebook.Application.SubAction.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.SubAction;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class SubActionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddSubActionRequest, AddSubActionCommand>()
           .Map(dest => dest.Name, src => src.Name)
           .Map(dest => dest.ActionId, src => src.ActionId);

        config.NewConfig<DeleteRequest, DeleteSubActionCommand>()
            .Map(dest => dest.Id, src => src.Id);
    }
}
