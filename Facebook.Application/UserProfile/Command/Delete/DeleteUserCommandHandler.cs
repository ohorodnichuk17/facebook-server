using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.TypeExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Command.Delete;

public class DeleteUserCommandHandler :
 IRequestHandler<DeleteUserCommand, ErrorOr<bool>>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    public DeleteUserCommandHandler(IUserProfileRepository userProfileRepository, ILogger<DeleteUserCommandHandler> logger)
    {
        _userProfileRepository = userProfileRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting delete user process...");
            var user = await _userProfileRepository.DeleteUserProfileAsync(request.UserId);
            if (user.IsSuccess())
            {
                _logger.LogInformation("Delete user process completed successfully");
                return true;
            }
            else
            {
                return Error.NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user delete");
            throw;
        }
    }
}
