using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReactionController(ISender mediatr, IMapper mapper) : ApiController
{
    public IActionResult Index()
    {
        return Ok();
    }
}
