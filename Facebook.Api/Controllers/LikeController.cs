using Facebook.Application.Like.Command.Add;
using Facebook.Application.Like.Command.Delete;
using Facebook.Application.Like.Query.GetAll;
using Facebook.Application.Like.Query.GetById;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Like.Add;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/like")]
[ApiController]
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
        catch (Exception ex)
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
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching likes.");
        }
    }
}
