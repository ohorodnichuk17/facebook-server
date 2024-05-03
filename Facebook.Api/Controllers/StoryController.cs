using Facebook.Application.Story.Command.Create;
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
}