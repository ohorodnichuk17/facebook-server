using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Feeling.Command.Add;

public class AddFeelingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddFeelingCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(AddFeelingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var feeling = new FeelingEntity
            {
                Name = request.Name,
                Emoji = request.Emoji
            };
            
            var result = await unitOfWork.Feeling.CreateAsync(feeling);

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