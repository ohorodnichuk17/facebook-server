using Facebook.Application.Chat.Command.Delete;
using Facebook.Application.Chat.Query.GetChatsByUserId;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Facebook.Server.Controllers;

[Route("api/chat")]
[ApiController]
public class ChatController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        try
        {
            var query = new GetChatsByUserIdQuery(userId);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var chats = res.Value;

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(chats, options);

                return Ok(json);
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
