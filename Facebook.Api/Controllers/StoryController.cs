using Facebook.Application.Story.Command.Create;
using Facebook.Application.Story.Command.Delete;
using Facebook.Application.Story.Query.GetAll;
using Facebook.Application.Story.Query.GetById;
using Facebook.Contracts.Story.Create;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StoryController : ApiController 
{
    private readonly ISender _mediatr;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    
    public StoryController(ISender mediatr, IMapper mapper, IConfiguration configuration)
    {
        _mediatr = mediatr;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(CreateStoryRequest request)
    {
        byte[] image = null;
        if (request.Image != null && request.Image.Length > 0)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await request.Image.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }
        }
        
        // var createStoryCommand = _mapper.Map<CreateStoryCommand>(request);
        // var createStoryResult = await _mediatr.Send(createStoryCommand); 
        var createStoryResult = await _mediatr.Send(_mapper
            .Map<CreateStoryCommand>((request, image)));

        return createStoryResult.Match(
            success => Ok(success), 
            error => Problem(error)); 
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