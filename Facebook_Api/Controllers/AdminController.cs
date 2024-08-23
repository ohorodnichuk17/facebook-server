using Facebook.Application.Admin.Command.BanUser;
using Facebook.Application.Admin.Command.BlockUser;
using Facebook.Application.Admin.Command.UnBanUser;
using Facebook.Application.Admin.Command.UnBlockUser;
using Facebook.Application.Admin.Query.GetAllUsers;
using Facebook.Application.Admin.Query.GetUserByEmail;
using Facebook.Application.Admin.Query.GetUserById;
using Facebook.Application.Comment.Command.Delete;
using Facebook.Application.Comment.Query.GetAll;
using Facebook.Application.Post.Command.Delete;
using Facebook.Application.Post.Query.GetAll;
using Facebook.Application.Story.Command.Delete;
using Facebook.Application.Story.Query.GetAll;
using Facebook.Application.UserProfile.Command.DeleteUser;
using Facebook.Contracts.Admin.Base;
using Facebook.Contracts.Admin.BlockAndUnblockUser;
using Facebook.Contracts.Admin.GetUserByEmail;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Constants.Roles;
using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Facebook.Domain.User;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Server.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class AdminController(ISender mediatr, IMapper mapper, UserManager<UserEntity> userManager)
    : ApiController
{
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteProfileAsync([FromBody] DeleteRequest request)
    {
        var command = mapper.Map<DeleteUserCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
            deleteRes => Ok(),
            errors => Problem(errors));
    }

    [HttpGet("get-user-by-email")]
    public async Task<IActionResult> GetUserByEmailAsync([FromQuery] GetUserByEmailRequest request)
    {
        var query = mapper.Map<GetUserByEmailQuery>(request);
        var result = await mediatr.Send(query);

        return result.Match(
            user => Ok(user),
            errors => Problem(errors));
    }

    [HttpGet("get-user-by-id")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var result = await mediatr.Send(query);

            return result.Match(
            user => Ok(user),
            errors => Problem(errors));
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting user.");
        }
    }
    
    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        var userRoles = await userManager.GetUsersInRoleAsync("Admin");
        var usersWithRoles = users.Select(user => new
        {
            user.Id,
            user.Email,
            Role = userRoles.Any(ur => ur.Id == user.Id) ? "Admin" : "User"
        }).ToList();

        return Ok(usersWithRoles);
    }
    
    [HttpPost("block-user")]
    public async Task<IActionResult> BlockUserAsync([FromBody] BlockAndUnblockUserRequest request)
    {
        var command = new BlockUserCommand(request.UserId);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(),
            errors => Problem(errors));
    }

    [HttpPost("unblock-user")]
    public async Task<IActionResult> UnBlockUserAsync([FromBody] BlockAndUnblockUserRequest request)
    {
        var command = new UnBlockUserCommand(request.UserId);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(),
            errors => Problem(errors));
    }

    [HttpPost("ban-user")]
    public async Task<IActionResult> BanUserAsync([FromBody] BaseAdminRequest request)
    {
        var command = new BanUserCommand(request.Id);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(),
            errors => Problem(errors));
    }

    [HttpPost("unban-user")]
    public async Task<IActionResult> UnBanUserAsync([FromBody] BaseAdminRequest request)
    {
        var command = new UnBanUserCommand(request.Id);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(),
            errors => Problem(errors));
    }

    [HttpDelete("delete-post")]
    public async Task<IActionResult> DeletePostAsync([FromBody] BaseAdminRequest request)
    {
        var command = new DeletePostCommand(request.Id);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(),
            errors => Problem(errors));
    }

    [HttpDelete("delete-comment")]
    public async Task<IActionResult> DeleteCommentAsync([FromBody] BaseAdminRequest request)
    {
        var command = new DeleteCommentCommand(request.Id);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(),
            errors => Problem(errors));
    }

    [HttpDelete("delete-story")]
    public async Task<IActionResult> DeleteStoryAsync([FromBody] BaseAdminRequest request)
    {
        var command = new DeleteStoryCommand(request.Id);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(),
            errors => Problem(errors));
    }

    [HttpGet("get-all-posts")]
    public async Task<IActionResult> GetAllPosts()
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

    [HttpGet("get-all-stories")]
    public async Task<IActionResult> GetAllStories()
    {
        try
        {
            var query = new GetAllStoriesQuery();
            var stories = await mediatr.Send(query);

            var mappedStories = stories.Value.Adapt<List<StoryEntity>>();

            return Ok(mappedStories);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching stories.");
        }
    }
    
    [HttpGet("get-all-comments")]
    public async Task<IActionResult> GetAllComments()
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