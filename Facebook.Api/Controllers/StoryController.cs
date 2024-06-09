using Facebook.Application.Story.Command.Create;
using Facebook.Application.Story.Command.Delete;
using Facebook.Application.Story.Query.GetAll;
using Facebook.Application.Story.Query.GetById;
using Facebook.Contracts.Story.Create;
using Facebook.Contracts.Story.Delete;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class StoryController(ISender mediatr, IMapper mapper, IConfiguration configuration) : ApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateStoryRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }

        byte[] image = null;
        if (request.Image != null && request.Image.Length > 0)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await request.Image.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }

        try
        {
            var command = mapper.Map<CreateStoryCommand>((request, image));
            var createStoryResult = await mediatr.Send(command);

            return createStoryResult.Match(
                success => Ok(success),
                error => Problem(error));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(DeleteStoryRequest request)
    {
        var command = mapper.Map<DeleteStoryCommand>(request);
        var deleteStoryResult = await mediatr.Send(command);

        return deleteStoryResult.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllStoriesQuery();
            var stories = await mediatr.Send(query);

            return Ok(stories.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching stories.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var query = new GetStoryByIdQuery(id);
            var storyResult = await mediatr.Send(query);

            if (storyResult.IsSuccess())
            {
                var story = storyResult.Value;
                if (story == null)
                {
                    return NotFound();
                }
                return Ok(story);
            }
            else
            {
                return StatusCode(500, storyResult.IsError);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while getting story.");
        }
    }
}