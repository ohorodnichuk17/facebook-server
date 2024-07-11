using Facebook.Application.Like.Command.Add;
using Facebook.Application.Like.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Like.Create;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/like")]
[ApiController]
[AllowAnonymous]
public class LikeController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("add")]
    public async Task<IActionResult> AddLikeAsync([FromForm] AddLikeRequest request)
    {
        var command = mapper.Map<AddLikeCommand>(request);
        var addResult = await mediatr.Send(command);

        return addResult.Match(
        success => Ok(success),
        errors => Problem(errors));
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteLikeAsync([FromForm] DeleteRequest request)
    {
        var command = mapper.Map<DeleteLikeCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
        deleteRes => Ok(),
        errors => Problem(errors));
    }
}
