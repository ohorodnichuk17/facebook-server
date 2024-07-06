using Facebook.Application.Post.Command.Create;
using Facebook.Application.Post.Command.Delete;
using Facebook.Application.Post.Query.GetAll;
using Facebook.Application.Post.Query.GetById;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Post.Create;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(ISender mediatr, IMapper mapper, IConfiguration configuration) : ApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromForm] CreatePostRequest request)
    {
        var command = mapper.Map<CreatePostCommand>(request);

        var createPostResult = await mediatr.Send(command);

        return createPostResult.Match(
           success => Ok(success),
           error => Problem(error));
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(DeleteRequest request)
    {
        var command = mapper.Map<DeletePostCommand>(request);
        var deletePostResult = await mediatr.Send(command);

        return deletePostResult.Match(
            success => Ok(success),
            error => Problem(error));
    }
    
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllPostsQuery();
            var posts = await mediatr.Send(query);

            return Ok(posts.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching posts.");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var query = new GetPostByIdQuery(id);
            var postResult = await mediatr.Send(query);

            if (postResult.IsSuccess())
            {
                var post = postResult.Value;
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(post);
            }
            else
            {
                return StatusCode(500, postResult.IsError);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while getting post.");
        }
    }
}