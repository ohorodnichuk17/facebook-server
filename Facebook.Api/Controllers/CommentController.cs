using Facebook.Application.Comment.Command.Add;
using Facebook.Application.Comment.Command.Delete;
using Facebook.Contracts.Comment.Create;
using Facebook.Contracts.DeleteRequest;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("add")]
    public async Task<IActionResult> AddCommentAsync([FromForm] AddCommentRequest request)
    {
        var command = mapper.Map<AddCommentCommand>(request);
        var addResult = await mediatr.Send(command);

        return addResult.Match(
        success => Ok(success),
        errors => Problem(errors));
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteCommentAsync([FromForm] DeleteRequest request)
    {
        var command = mapper.Map<DeleteCommentCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
        deleteRes => Ok(),
        errors => Problem(errors));
    }
}
