using Facebook.Application.Like.Command.Add;
using Facebook.Application.Like.Command.Delete;
using Facebook.Application.Like.Query.GetAll;
using Facebook.Application.Like.Query.GetById;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Like.Add;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/like")]
[ApiController]
public class LikeController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddLikeAsync([FromBody] LikeRequest request)
    {
        var command = mapper.Map<AddLikeCommand>(request);
        var addResult = await mediatr.Send(command);

        return addResult.Match(Ok, Problem);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLikeByPostIdAsync([FromBody] LikeRequest request)
    {
        var command = mapper.Map<DeleteLikeByPostIdCommand>(request);
        var result = await mediatr.Send(command);
        return result.Match(
            result => Ok(),
            Problem);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var query = new GetLikeByIdQuery(id);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var like = res.Value;
                if (like == null)
                {
                    return NotFound();
                }
                return Ok(like);
            }
            else
            {
                return StatusCode(500, res.IsError);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting like.");
        }
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllLikesQuery();
            var like = await mediatr.Send(query);

            return Ok(like.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching likes.");
        }
    }
}
