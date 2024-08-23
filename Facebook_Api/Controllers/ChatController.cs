using Facebook.Application.Chat.Command.Create;
using Facebook.Application.Chat.Command.Delete;
using Facebook.Application.Chat.Query.GetChatsByUserId;
using Facebook.Contracts.Chat;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Chat;
using Facebook.Domain.TypeExtensions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/chat")]
[ApiController]
public class ChatController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
    {
        var command = mapper.Map<CreateChatCommand>(request);
        var result = await mediatr.Send(command);

        return result.IsError ? BadRequest() : Created();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        try
        {
            var query = new GetChatsByUserIdQuery(userId);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var chats = res.Value.Adapt<List<ChatEntity>>();

                return Ok(chats);
            }
            return StatusCode(500, res.IsError);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting messages.");
        }
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(DeleteRequest request)
    {
        var command = mapper.Map<DeleteChatByIdCommand>(request);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }
}
