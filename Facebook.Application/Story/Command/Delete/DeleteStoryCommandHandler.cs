using MediatR;
using ErrorOr;
using Facebook.Application.Common.Interfaces.IUnitOfWork;

namespace Facebook.Application.Story.Command.Delete;

public class DeleteStoryCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteStoryCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await unitOfWork.Story.DeleteAsync(request.Id);
        
            if (result.IsError)
            {
                return result.Errors;
            }
        
            return true;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}