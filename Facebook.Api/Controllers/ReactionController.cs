using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class ReactionController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpGet("get-reaction")]
    public async Task<IActionResult> GetReaction()
    {
        return Ok();
    }
}
