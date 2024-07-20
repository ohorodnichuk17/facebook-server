using Facebook.Application.Message.Query.GetMessagesById;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/message")]
[ApiController]
public class MessageController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetById(Guid chatId)
    {
        try
        {
            var query = new GetMessagesByIdQuery(chatId);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var comment = res.Value;
                if (comment == null)
                {
                    return NotFound();
                }
                return Ok(comment);
            }
            else
            {
                return StatusCode(500, res.IsError);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while getting messages.");
        }
    }
}
