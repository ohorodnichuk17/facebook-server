using Facebook.Application.SubAction.Command.Add;
using Facebook.Application.SubAction.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.SubAction;
using Facebook.Domain.Post;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class SubActionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddSubActionRequest, AddSubActionCommand>();

        config.NewConfig<DeleteRequest, DeleteSubActionCommand>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<AddSubActionCommand, SubActionEntity>();
    }
}
