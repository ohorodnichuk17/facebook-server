using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Query.GetById;

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, ErrorOr<UserProfileEntity>>
{
    private readonly IUserProfileRepository _userProfileRepository;
    public GetUserProfileByIdQueryHandler(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }
    public async Task<ErrorOr<UserProfileEntity>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getUserProfile = await _userProfileRepository.GetUserProfileByIdAsync(request.UserId.ToString());

            if (getUserProfile.IsError)
            {
                return Error.Failure(getUserProfile.Errors.ToString() ?? string.Empty);
            }
            return getUserProfile.Value;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
