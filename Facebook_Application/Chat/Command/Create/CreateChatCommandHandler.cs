using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Chat;
using MediatR;

namespace Facebook.Application.Chat.Command.Create;

public class CreateChatCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateChatCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.GetUserByIdAsync(request.UserId);
        var friend = await unitOfWork.User.GetUserByIdAsync(request.FriendId);

        if (user.IsError || friend.IsError)
        {
            return Error.NotFound();
        }

        var chat = new ChatEntity
        {
            Name = friend.Value.Email ?? "Unknown",
            ChatUsers =
                [
                    new ChatUserEntity { UserId = user.Value.Id },
                    new ChatUserEntity { UserId = friend.Value?.Id ?? Guid.Empty }
                ]
        };

        var result = await unitOfWork.Chat.CreateAsync(chat);

        return result;
    }
}