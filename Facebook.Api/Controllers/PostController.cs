using AutoMapper;
using Facebook.Application.Post.Command.Create;
using Facebook.Contracts.Post.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(ISender mediatr, IMapper mapper, IConfiguration configuration) : ApiController
{
   [HttpPost("create")]
   public async Task<IActionResult> CreateAsync([FromForm] CreatePostRequest request)
   {
      var command = mapper.Map<CreatePostCommand>(request);

      var createPostResult = await mediatr.Send(command);

      return createPostResult.Match(
         success => Ok(success),
         error => Problem(error));
   }
}