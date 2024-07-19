using Facebook.Application.UserProfile.Command.DeleteUser;
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
}