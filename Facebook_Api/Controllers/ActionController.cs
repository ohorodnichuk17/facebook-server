using Facebook.Application.Action.Command.Add;
using Facebook.Application.Action.Command.Delete;
using Facebook.Application.Action.Query.GetAll;
using Facebook.Application.Action.Query.GetById;
using Facebook.Contracts.Action.Add;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/action")]
[ApiController]
public class ActionController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("add")]
    public async Task<IActionResult> AddAsync(AddActionRequest request)
    {
        try
        {
            var command = mapper.Map<AddActionCommand>(request);
            var result = await mediatr.Send(command);

            return result.Match(
                success => Ok(success),
                error => Problem(error));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(DeleteRequest request)
    {
        var command = mapper.Map<DeleteActionCommand>(request);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var query = new GetActionByIdQuery(id);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var action = res.Value;

                return Ok(action);
            }
            return StatusCode(500, res.IsError);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting action.");
        }
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllActionsQuery();
            var action = await mediatr.Send(query);

            return Ok(action.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching actions.");
        }
    }
}
