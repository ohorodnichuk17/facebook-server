using Facebook.Application.Chat.Command.Create;
using Facebook.Application.Chat.Command.Delete;
using Facebook.Application.DTO;
using Facebook.Contracts.Chat;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Chat;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class ChatMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateChatRequest, CreateChatCommand>();

        config.NewConfig<DeleteRequest, DeleteChatByIdCommand>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<ChatEntity, ChatEntity>()
            .Map(dest => dest.Messages, src => src.Messages.Adapt<List<MessageDto>>())
            .Map(dest => dest.ChatUsers, src => src.ChatUsers.Adapt<List<ChatUserDto>>());
    }
}
