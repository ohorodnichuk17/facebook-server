using AutoMapper;
using Facebook.Application.Story.Command.Create;
using Facebook.Application.Story.Command.Delete;
using Facebook.Application.Story.Query.GetAll;
using Facebook.Application.Story.Query.GetById;
using Facebook.Contracts.Story.Create;
using Facebook.Contracts.Story.Delete;
using Facebook.Domain.TypeExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StoryController(ISender _mediatr, IMapper _mapper, IConfiguration _configuration) : ApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateStoryRequest request)
    {
        var command = _mapper.Map<CreateStoryCommand>(request);
        var createStoryResult = await _mediatr.Send(command);

        return createStoryResult.Match(
            success => Ok(success),
            error => Problem(error));
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(DeleteStoryRequest request)
    {
        var command = _mapper.Map<DeleteStoryCommand>(request);
        var deleteStoryResult = await _mediatr.Send(command);

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
            var stories = await _mediatr.Send(query);

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
            var storyResult = await _mediatr.Send(query);

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