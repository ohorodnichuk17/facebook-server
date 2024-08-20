using Facebook.Application.Comment.Command.Add;
using Facebook.Application.Comment.Command.AddReplyComment;
using Facebook.Application.Comment.Command.Delete;
using Facebook.Application.Comment.Command.Edit;
using Facebook.Application.Comment.Query.GetAll;
using Facebook.Application.Post.Query.GetCommentByPostId;
using Facebook.Contracts.Comment.Add;
using Facebook.Contracts.Comment.AddReplyComment;
using Facebook.Contracts.Comment.Edit;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Post;
using Facebook.Domain.TypeExtensions;
using Mapster;
using MapsterMapper;
using MediatR;
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
        success => Ok(addResult.Value),
        Problem);
    }

    [HttpPost("add-reply")]
    public async Task<IActionResult> AddReplyCommentAsync([FromForm] AddReplyCommentRequest request)
    {
        var command = mapper.Map<AddReplyCommentCommand>(request);
        var addResult = await mediatr.Send(command);

        return addResult.Match(
            success => Ok(addResult.Value),
            Problem);
    }

    [HttpPut]
    public async Task<IActionResult> EditAsync([FromBody] EditCommentRequest request)
    {
        var command = mapper.Map<EditCommentCommand>(request);
        var editResult = await mediatr.Send(command);

        return editResult.Match(
            authResult => Ok(editResult.Value),
            Problem);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCommentAsync([FromBody] DeleteRequest request)
    {
        var command = new DeleteCommentCommand(request.Id);
        var result = await mediatr.Send(command);

        return result.Match(
            result => Ok(),
            Problem);
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetById(Guid postId)
    {
        try
        {
            var query = new GetCommentsByPostIdQuery(postId);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var comments = res.Value.Adapt<List<CommentEntity>>();

                return Ok(comments);
            }
            else
            {
                return StatusCode(500, res.IsError);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting comment.");
        }
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllCommentsQuery();
            var comment = await mediatr.Send(query);

            return Ok(comment.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching comments.");
        }
    }
}
