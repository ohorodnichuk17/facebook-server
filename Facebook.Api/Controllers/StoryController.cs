using Facebook.Application.Story.Command.Create;
using Facebook.Application.Story.Command.Delete;
using Facebook.Application.Story.Query.GetAll;
using Facebook.Application.Story.Query.GetById;
using Facebook.Application.Story.Query.GetFriendsStories;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Story.Create;
using Facebook.Domain.Story;
using Facebook.Domain.TypeExtensions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/story")]
[ApiController]
public class StoryController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateStoryRequest? request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }

        byte[] image = [];
        if (request.Image != null && request.Image.Length > 0)
        {
            using MemoryStream memoryStream = new();
            await request.Image.CopyToAsync(memoryStream);
            image = memoryStream.ToArray();
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

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var command = new DeleteStoryCommand { Id = id };
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

            var mappedStories = stories.Value.Adapt<List<StoryEntity>>();

            return Ok(mappedStories);
        }
        catch (Exception)
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
                return Ok(story);
            }
            return StatusCode(500, storyResult.IsError);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting story.");
        }
    }

    [HttpGet("friends")]
    public async Task<IActionResult> GetFriendsStories()
    {
        var query = new GetFriendsStoriesQuery();
        var result = await mediatr.Send(query);
        var mappedStories = result.Value.Adapt<List<StoryEntity>>();

        return result.Match(result => Ok(mappedStories), Problem);
    }
}