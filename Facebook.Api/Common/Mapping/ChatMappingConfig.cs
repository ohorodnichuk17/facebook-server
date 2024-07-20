using Facebook.Application.Chat.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class ChatMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DeleteRequest, DeleteChatByIdCommand>()
            .Map(dest => dest.Id, src => src.Id);
    }
}
