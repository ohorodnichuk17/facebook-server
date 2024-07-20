using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.User;
using MediatR;

namespace Facebook.Application.Admin.Query.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserByEmailQuery, ErrorOr<UserEntity>>
{
    public async Task<ErrorOr<UserEntity>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await unitOfWork.Admin.GetUserByEmailAsync(request.Email);

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