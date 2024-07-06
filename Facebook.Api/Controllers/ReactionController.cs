using Facebook.Application.Reaction.Command.Add;
using Facebook.Application.Reaction.Command.Delete;
using Facebook.Contracts.Reaction.Add;
using Facebook.Contracts.Reaction.Delete;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/reaction")]
[ApiController]
public class ReactionController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("add-reaction")]
    public async Task<IActionResult> AddReactionAsync([FromForm] AddReactionRequest request)
    {
        var command = mapper.Map<AddReactionCommand>(request);
        var addResult = await mediatr.Send(command);

        return addResult.Match(
        success => Ok(success),
        errors => Problem(errors));
    }
    [HttpDelete("delete-reaction")]
    public async Task<IActionResult> DeleteReactionAsync([FromForm] DeleteReactionRequest request)
    {
        var command = mapper.Map<DeleteReactionCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
        deleteRes => Ok(),
        errors => Problem(errors));
    }
}
