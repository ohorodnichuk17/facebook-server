using Facebook.Application.Admin.Command.BanUser;
using Facebook.Application.Admin.Command.BlockUser;
using Facebook.Application.Admin.Command.UnBanUser;
using Facebook.Application.Admin.Command.UnBlockUser;
using Facebook.Application.Admin.Query.GetAllUsers;
using Facebook.Application.Admin.Query.GetUserByEmail;
using Facebook.Application.Admin.Query.GetUserById;
using Facebook.Application.Comment.Command.Delete;
using Facebook.Application.Post.Command.Delete;
using Facebook.Application.Story.Command.Delete;
using Facebook.Application.UserProfile.Command.DeleteUser;
using Facebook.Contracts.Admin.Base;
using Facebook.Contracts.Admin.BlockAndUnblockUser;
using Facebook.Contracts.Admin.GetUserByEmail;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Constants.Roles;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class AdminController(ISender mediatr, IMapper mapper, IConfiguration configuration)
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
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while getting user.");
        }
    }

    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var users = await mediatr.Send(query);

            return Ok(users.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching users.");
        }
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
}