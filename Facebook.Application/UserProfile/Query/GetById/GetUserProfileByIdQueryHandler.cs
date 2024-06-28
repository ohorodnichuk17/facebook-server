using ErrorOr;
using Facebook.Application.Common.Interfaces.Persistance;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.UserProfile.Query.GetById;

public class GetUserProfileByIdQueryHandler(IUserProfileRepository userProfileRepository)
    : IRequestHandler<GetUserProfileByIdQuery, ErrorOr<UserProfileEntity>>
{
    public async Task<ErrorOr<UserProfileEntity>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getUserProfile = await userProfileRepository.GetUserProfileByIdAsync(request.UserId.ToString());

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
