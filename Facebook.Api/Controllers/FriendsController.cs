using System.Text.Json;
using System.Text.Json.Serialization;
using Facebook.Application.Story.Query.GetAll;
using Facebook.Application.User.Friends.Command.AcceptFriendRequest;
using Facebook.Application.User.Friends.Command.RejectFriendRequest;
using Facebook.Application.User.Friends.Command.RemoveFriend;
using Facebook.Application.User.Friends.Command.SendFriendRequest;
using Facebook.Application.User.Friends.Query.GetAll;
using Facebook.Application.User.Friends.Query.GetById;
using Facebook.Contracts.Friends.AcceptFriendRequest;
using Facebook.Contracts.Friends.RejectFriendRequest;
using Facebook.Contracts.Friends.RemoveFriend;
using Facebook.Contracts.Friends.SendFriendRequest;
using Facebook.Domain.TypeExtensions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class FriendsController(ISender mediatr, IMapper mapper, IConfiguration configuration) : ApiController
{
   [HttpPost("accept-friend-request")]
   public async Task<IActionResult> AcceptFriendRequest(AcceptFriendRequestRequest request)
   {
      var result = await mediatr.Send(mapper.Map<AcceptFriendRequestCommand>(request));
    
      return result.Match(
         success => Ok(success),
         error => 
         {
            Console.Error.WriteLine($"Error in AcceptFriendRequest: {error}");
            return Problem(error.First().Description);
         });
   }
   
   [HttpPost("send-friend-request")]
   public async Task<IActionResult> SendFriendRequest(SendFriendRequestRequest request)
   {
      var result = await mediatr.Send(mapper.Map<SendFriendRequestCommand>(request));
      
      return result.Match(
         success => Ok(success),
         error => Problem(error));
   }
   
   [HttpPost("reject-friend-request")]
   public async Task<IActionResult> RejectFriendRequest(RejectFriendRequestRequest request)
   {
      var result = await mediatr.Send(mapper.Map<RejectFriendRequestCommand>(request));
    
      return result.Match(
         success => Ok(success),
         error => 
         {
            Console.Error.WriteLine($"Error in RejectFriendRequest: {error}");
            return Problem(error.First().Description);
         });
   }
   
   [HttpPost("remove-friend")]
   public async Task<IActionResult> RemoveFriend(RemoveFriendRequest request)
   {
      var result = await mediatr.Send(mapper.Map<RemoveFriendCommand>(request));
      
      return result.Match(
         success => Ok(success),
         error => Problem(error));
   }

   [HttpGet("get-all-friends")]
   public async Task<IActionResult> GetAllFriends([FromQuery] Guid userId)
   {
      try
      {
         var query = new GetAllFriendsQuery(userId);
         var friends = await mediatr.Send(query);

         return Ok(friends.Value);
      }
      catch (Exception ex)
      {
         return StatusCode(500, "An error occurred while fetching friends.");
      }
   }

   [HttpGet("get-friend-by-id")]
   public async Task<IActionResult> GetFriendById([FromQuery] Guid userId, Guid friendId)
   {
      try
      {
         var query = new GetFriendByIdQuery(userId, friendId);
         var result = await mediatr.Send(query);

         if (result.IsSuccess())
         {
            var friend = result.Value;
            if (friend == null)
            {
               return NotFound();
            }

            var options = new JsonSerializerOptions
            {
               ReferenceHandler = ReferenceHandler.Preserve,
               WriteIndented = true
            };

            var json = JsonSerializer.Serialize(friend, options);

            return Ok(json);
         }
         else
         {
            return StatusCode(500, result.IsError);
         }
      }
      catch (Exception ex)
      {
         return StatusCode(500, "An error occurred while getting friend.");
      }
   }
}