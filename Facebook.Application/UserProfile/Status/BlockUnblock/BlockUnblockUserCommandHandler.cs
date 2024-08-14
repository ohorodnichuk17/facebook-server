using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.TypeExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Status.Block;

public class BlockUnblockUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<BlockUnblockUserCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(BlockUnblockUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userProfile = await unitOfWork.UserProfile.GetUserProfileByIdAsync(request.UserId.ToString());

            if (userProfile.IsError)
            {
                return userProfile.Errors;
            }

            if (userProfile.Value.IsBlocked == false)
            {
                var blockRes = await unitOfWork.UserProfile.BlockUserAsync(userProfile.Value.UserId);

                if (blockRes.IsSuccess())
                {
                    return userProfile.Value.IsBlocked;
                }
                else
                {
                    return Error.NotFound();
                }
            }
            else
            {
                var unblockRes = await unitOfWork.UserProfile.UnblockUserAsync(userProfile.Value.UserId);

                if (unblockRes.IsSuccess())
                {
                    return userProfile.Value.IsBlocked;
                }
                else
                {
                    return Error.NotFound();
                }
            }
            
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
