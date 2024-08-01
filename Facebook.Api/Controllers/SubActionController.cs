using Facebook.Application.SubAction.Command.Add;
using Facebook.Application.SubAction.Command.Delete;
using Facebook.Application.SubAction.Query.GetAll;
using Facebook.Application.SubAction.Query.GetById;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.SubAction;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/subAction")]
[ApiController]
public class SubActionController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("add")]
    public async Task<IActionResult> AddAsync(AddSubActionRequest request)
    {
        try
        {
            var command = mapper.Map<AddSubActionCommand>(request);
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
        var command = mapper.Map<DeleteSubActionCommand>(request);
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
            var query = new GetSubActionByIdQuery(id);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var subAction = res.Value;
                if (subAction == null)
                {
                    return NotFound();
                }
                return Ok(subAction);
            }
            return StatusCode(500, res.IsError);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting subAction.");
        }
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllSubActionsQuery();
            var subAction = await mediatr.Send(query);

            return Ok(subAction.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching subActions.");
        }
    }
}
