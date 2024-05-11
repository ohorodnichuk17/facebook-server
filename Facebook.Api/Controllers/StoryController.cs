using AutoMapper;
using Facebook.Application.Story.Command.Create;
using Facebook.Contracts.Story.Create;
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
}