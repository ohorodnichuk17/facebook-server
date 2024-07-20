using Facebook.Application.Admin.Query.GetAllUsers;
using Facebook.Application.Admin.Query.GetUserByEmail;
using Facebook.Application.Admin.Query.GetUserById;
using Facebook.Application.UserProfile.Command.DeleteUser;
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
    public async Task<IActionResult> DeleteProfileAsync([FromForm] DeleteRequest request)
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
}