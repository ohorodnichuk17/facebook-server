using Facebook.Application.Reaction.Command.Add;
using Facebook.Application.Reaction.Command.Delete;
using Facebook.Application.Reaction.Query.GetAll;
using Facebook.Application.Reaction.Query.GetById;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Reaction.Add;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/reaction")]
[ApiController]
public class ReactionController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddReactionAsync([FromBody] AddReactionRequest request)
    {
        var command = mapper.Map<AddReactionCommand>(request);
        var addResult = await mediatr.Send(command);

        return addResult.Match(
        success => Ok(success),
        Problem);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var query = new GetReactionByIdQuery(id);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var reaction = res.Value;
                return Ok(reaction);
            }
            else
            {
                return StatusCode(500, res.IsError);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting reaction.");
        }
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllReactionsQuery();
            var reaction = await mediatr.Send(query);

            return Ok(reaction.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching reactions.");
        }
    }
}
