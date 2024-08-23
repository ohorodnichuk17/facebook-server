using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Admin.Query.GetUserById;

public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetUserByIdQuery, ErrorOr<UserEntity>>
{
    public async Task<ErrorOr<UserEntity>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await unitOfWork.User.GetUserByIdAsync(request.Id);

            if (user.IsError)
            {
                return user.Errors;
            }

            return user;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}