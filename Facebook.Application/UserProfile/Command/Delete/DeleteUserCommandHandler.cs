using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.TypeExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Facebook.Application.UserProfile.Command.Delete;

public class DeleteUserCommandHandler(
    IUserProfileRepository userProfileRepository,
    ILogger<DeleteUserCommandHandler> logger)
    :
        IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Starting delete user process...");
            var user = await userProfileRepository.DeleteUserProfileAsync(request.UserId);
            if (user.IsSuccess())
            {
                logger.LogInformation("Delete user process completed successfully");
                return true;
            }
            else
            {
                return Error.NotFound();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user delete");
            throw;
        }
    }
}
