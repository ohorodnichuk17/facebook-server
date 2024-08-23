using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Admin.Query.GetAllUsers;

public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllUsersQuery, ErrorOr<List<UserEntity>>>
{
    public async Task<ErrorOr<List<UserEntity>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await unitOfWork.User.GetAllUsersAsync();

            if (users.IsError)
            {
                return users.Errors;
            }

            return users;
        }
        catch (Exception e)
        {
            return Error.Failure(e.Message);
        }
    }
}