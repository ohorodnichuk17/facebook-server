using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MapsterMapper;
using MediatR;

namespace Facebook.Application.SubAction.Command.Add;

public class AddSubActionCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<AddSubActionCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(AddSubActionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var subAction = mapper.Map<SubActionEntity>(request);

            var result = await unitOfWork.SubAction.CreateAsync(subAction);

            if (result.IsError)
            {
                return result.Errors;
            }

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
