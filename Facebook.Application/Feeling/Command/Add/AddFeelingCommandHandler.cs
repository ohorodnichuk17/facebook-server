using ErrorOr;
using Facebook.Application.Common.Interfaces.Feeling.IRepository;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Feeling.Command.Add;

public class AddFeelingCommandHandler(IFeelingRepository feelingRepository)
    : IRequestHandler<AddFeelingCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(AddFeelingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var feeling = new FeelingEntity
            {
                // Id = new Guid(),
                Name = request.Name,
                Emoji = request.Emoji
            };
            
            var result = await feelingRepository.AddFeelingAsync(feeling);

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