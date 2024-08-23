using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Application.DTO_s;
using MediatR;

namespace Facebook.Application.User.Friends.Query.SearchByFirstAndLastNames;

public class SearchByFirstAndLastNamesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SearchByFirstAndLastNamesQuery, ErrorOr<List<UserDto>>>
{
    public async Task<ErrorOr<List<UserDto>>> Handle(SearchByFirstAndLastNamesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var friends = await unitOfWork.User.SearchUsersByFirstNameAndLastNameAsync(request.FirstName, request.LastName);

            if (friends.IsError)
            {
                return friends.Errors;
            }

            return friends;
        }
        catch (Exception ex)
        {
            return Error.Failure($"Error while retrieving users: {ex.Message}");
        }
    }
}