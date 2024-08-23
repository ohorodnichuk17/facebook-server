using Facebook.Application.Post.Command.Create;
using Facebook.Application.Post.Command.Delete;
using Facebook.Application.Post.Query.GetAll;
using Facebook.Application.Post.Query.GetById;
using Facebook.Application.Post.Query.GetCommentByPostId;
using Facebook.Application.Post.Query.GetFriendsPosts;
using Facebook.Application.Post.Query.GetLikeByPostId;
using Facebook.Application.Post.Query.GetPostAccess;
using Facebook.Application.Post.Query.GetReactionByPostId;
using Facebook.Application.Post.Query.SearchPostsByTags;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Post.Create;
using Facebook.Domain.Post;
using Facebook.Domain.TypeExtensions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Facebook.Server.Controllers;

[Route("api/post")]
[ApiController]
[AllowAnonymous]
public class PostController(ISender mediatr, IMapper mapper) : ApiController
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

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromBody] DeleteRequest request)
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

            var mappedPosts = posts.Value.Adapt<List<PostEntity>>();

            return Ok(mappedPosts);
        }
        catch (Exception)
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
                return Ok(post);
            }
            else
            {
                return StatusCode(500, postResult.IsError);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting post.");
        }
    }

    [HttpGet("getLikesBy/{postId}")]
    public async Task<IActionResult> GetLikesByPostId(Guid postId)
    {
        try
        {
            var query = new GetLikesByPostIdQuery(postId);
            var postResult = await mediatr.Send(query);

            if (postResult.IsSuccess())
            {
                var post = postResult.Value;
                return Ok(post);
            }
            else
            {
                return StatusCode(500, postResult.IsError);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting post.");
        }
    }

    [HttpGet("getCommentsBy/{postId}")]
    public async Task<IActionResult> GetCommentsByPostId(Guid postId)
    {
        try
        {
            var query = new GetCommentsByPostIdQuery(postId);
            var postResult = await mediatr.Send(query);

            if (postResult.IsSuccess())
            {
                var post = postResult.Value;
                return Ok(post);
            }
            return StatusCode(500, postResult.IsError);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting post.");
        }
    }

    [HttpGet("getReactionsBy/{postId}")]
    public async Task<IActionResult> GetReactionsByPostId(Guid postId)
    {
        try
        {
            var query = new GetReactionsByPostIdQuery(postId);
            var postResult = await mediatr.Send(query);

            if (postResult.IsSuccess())
            {
                var post = postResult.Value;
                return Ok(post);
            }
            return StatusCode(500, postResult.IsError);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting post.");
        }
    }

    [HttpPost("search-posts-by-tags")]
    public async Task<IActionResult> SearchPostsByTags(string tag)
    {
        try
        {
            var query = new SearchPostsByTagsQuery(tag);
            var result = await mediatr.Send(query);

            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }

            var mappedPosts = result.Value.Adapt<List<PostEntity>>();

            return Ok(mappedPosts);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching posts.");
        }

    }

    [HttpGet("getPostAccess/{viewerId}/{postId}")]
    public async Task<IActionResult> CanViewPost(Guid viewerId, Guid postId)
    {
        try
        {
            var query = new GetPostAccessQuery(viewerId, postId);
            var postResult = await mediatr.Send(query);

            if (postResult.IsSuccess())
            {
                var post = postResult.Value;
                return Ok(post);
            }
            return StatusCode(500, postResult.IsError);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting post limited information.");
        }
    }

    [HttpGet("friends")]
    public async Task<IActionResult> GetFriendsPosts(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var query = new GetFriendsPostsQuery(pageNumber, pageSize);
            var paginationResponse = await mediatr.Send(query);

            paginationResponse.Value.Posts = paginationResponse.Value.Posts.Adapt<List<PostEntity>>();

            return Ok(paginationResponse.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching posts.");
        }
    }
}