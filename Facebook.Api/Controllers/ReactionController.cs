using Facebook.Application.Reaction.Command.Add;
using Facebook.Application.Reaction.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Reaction.Add;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/reaction")]
[ApiController]
public class ReactionController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("add")]
    public async Task<IActionResult> AddReactionAsync([FromForm] AddReactionRequest request)
    {
        var command = mapper.Map<AddReactionCommand>(request);
        var addResult = await mediatr.Send(command);

        return addResult.Match(
        success => Ok(success),
        errors => Problem(errors));
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteReactionAsync([FromForm] DeleteRequest request)
    {
        var command = mapper.Map<DeleteReactionCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
        deleteRes => Ok(),
        errors => Problem(errors));
    }
}
