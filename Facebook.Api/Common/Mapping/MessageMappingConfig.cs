using Facebook.Application.Message.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class MessageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteRequest, DeleteMessageByIdCommand>()
       .Map(dest => dest.Id, src => src.Id);
    }
}
