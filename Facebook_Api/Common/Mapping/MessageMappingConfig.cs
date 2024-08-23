using Facebook.Application.Message.Command.Delete;
using Facebook.Application.Message.Command.Edit;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Message.Edit;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class MessageMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteRequest, DeleteMessageByIdCommand>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<EditMessageRequest, EditMessageCommand>()
            .Map(dest => dest.Content, src => src.Content)
            .Map(dest => dest.Id, src => src.Id);
    }
}
