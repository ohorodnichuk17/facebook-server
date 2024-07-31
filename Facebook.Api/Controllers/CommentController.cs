using Facebook.Application.Comment.Command.Add;
using Facebook.Application.Comment.Command.AddReplyComment;
using Facebook.Application.Comment.Command.Delete;
using Facebook.Application.Comment.Command.Edit;
using Facebook.Application.Comment.Query.GetAll;
using Facebook.Application.Post.Query.GetCommentByPostId;
using Facebook.Contracts.Comment.AddReplyComment;
using Facebook.Contracts.Comment.Create;
using Facebook.Contracts.Comment.Edit;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(addResult, options);

        return Ok();
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditAsync([FromForm] EditCommentRequest request)
    {
        var editResult = await mediatr.Send(
            mapper.Map<EditCommentCommand>(request));

        return editResult.Match(
            authResult => Ok(editResult.Value),
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

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetById(Guid postId)
    {
        try
        {
            var query = new GetCommentsByPostIdQuery(postId);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var comment = res.Value;
                if (comment == null)
                {
                    return NotFound();
                }
                return Ok(comment);
            }
            else
            {
                return StatusCode(500, res.IsError);
            }
        }
        catch (Exception ex)
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
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching comments.");
        }
    }
}
