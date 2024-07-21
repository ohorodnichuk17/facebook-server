using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MapsterMapper;
using MediatR;

namespace Facebook.Application.Action.Command.Add;

public class AddActionCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<AddActionCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(AddActionCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var action = mapper.Map<ActionEntity>(request);

            var result = await unitOfWork.Action.CreateAsync(action);

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
