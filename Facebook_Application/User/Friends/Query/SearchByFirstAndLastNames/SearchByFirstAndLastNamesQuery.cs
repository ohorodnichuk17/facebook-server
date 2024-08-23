using ErrorOr;
using Facebook.Application.DTO_s;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.User.Friends.Query.SearchByFirstAndLastNames;

public record SearchByFirstAndLastNamesQuery(
    Guid UserId,
    string FirstName,
    string LastName
) : IRequest<ErrorOr<List<UserDto>>>;