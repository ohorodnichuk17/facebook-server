﻿using Facebook.Application.Message.Command.Delete;
using Facebook.Application.Message.Query.GetMessagesById;
using Facebook.Contracts.DeleteRequest;
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
    public async Task<IActionResult> GetById([FromQuery] Guid chatId)
    {
        try
        {
            var query = new GetMessagesByIdQuery(chatId);
            var res = await mediatr.Send(query);

            if (res.IsSuccess())
            {
                var messages = res.Value;
                if (messages == null)
                {
                    return NotFound();
                }

                return Ok(messages);
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
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(DeleteRequest request)
    {
        var command = mapper.Map<DeleteMessageByIdCommand>(request);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(success),
            error => Problem(error));
    }
}
