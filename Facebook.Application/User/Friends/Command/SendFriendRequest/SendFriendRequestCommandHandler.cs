using ErrorOr;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Application.Services;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Command.SendFriendRequest;

public class SendFriendRequestCommandHandler(
    IUserRepository userRepository,
    EmailService emailService) : IRequestHandler<SendFriendRequestCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await userRepository
                .SendFriendRequestAsync(request.UserId.ToString(), request.FriendId.ToString());

            if (result.IsError)
            {
                return result.Errors;
            }

            var findFriendResult = await userRepository.GetUserByIdAsync(request.FriendId);

            if (findFriendResult.IsError)
            {
                return findFriendResult.Errors;
            }
            var friend = findFriendResult.Value;

            var friendUserName = GetUserNameForEmail(friend);

            await emailService.SendFriendRequestNotificationEmailAsync(friend.Email, request.baseUrl, friendUserName, request.UserId);

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }

    private string GetUserNameForEmail(UserEntity user)
    {
        if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
        {
            return user.FirstName + " " + user.LastName;
        }
        if (string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.FirstName))
        {
            return user.Email;
        }
        else if (string.IsNullOrEmpty(user.LastName))
        {
            return user.FirstName;
        }
        else
        {
            return user.LastName;
        }
    }
}