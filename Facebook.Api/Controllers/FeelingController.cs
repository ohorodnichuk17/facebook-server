using Facebook.Application.Feeling.Command.Add;
using Facebook.Application.Feeling.Command.Delete;
using Facebook.Application.Feeling.Query.GetAll;
using Facebook.Application.Feeling.Query.GetById;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Feeling.Add;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/feeling")]
[ApiController]
public class FeelingController(ISender mediatr, IMapper mapper)
    : ApiController
{
    [HttpPost("add")]
    public async Task<IActionResult> AddAsync(AddFeelingRequest request)
    {
        try
        {
            var command = mapper.Map<AddFeelingCommand>(request);
            var result = await mediatr.Send(command);

            return result.Match(
                s => Ok(s),
                e => Problem(e));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(DeleteRequest request)
    {
        var command = mapper.Map<DeleteFeelingCommand>(request);
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
            var query = new GetFeelingByIdQuery(id);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var f = res.Value;
                return Ok(f);
            }
            else
            {
                return StatusCode(500, res.IsError);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting feeling.");
        }
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllFeelingsQuery();
            var f = await mediatr.Send(query);

            return Ok(f.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching feelings.");
        }
    }
}