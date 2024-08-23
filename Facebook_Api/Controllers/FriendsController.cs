using Facebook.Application.DTO_s;
using Facebook.Application.User.Friends.Command.AcceptFriendRequest;
using Facebook.Application.User.Friends.Command.RejectFriendRequest;
using Facebook.Application.User.Friends.Command.RemoveFriend;
using Facebook.Application.User.Friends.Command.SendFriendRequest;
using Facebook.Application.User.Friends.Query.GetAll;
using Facebook.Application.User.Friends.Query.GetAllFriendRequests;
using Facebook.Application.User.Friends.Query.GetById;
using Facebook.Application.User.Friends.Query.GetFriendsRecommendations;
using Facebook.Application.User.Friends.Query.GetRelationshipsStatus;
using Facebook.Application.User.Friends.Query.SearchByFirstAndLastNames;
using Facebook.Contracts.Friends;
using Facebook.Domain.TypeExtensions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Facebook.Server.Controllers;

[Route("api/friends")]
[ApiController]
[AllowAnonymous]
public class FriendsController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPost("accept-friend-request")]
    public async Task<IActionResult> AcceptFriendRequest([FromBody] FriendRequest request)
    {
        var command = new AcceptFriendRequestCommand(request.FriendId);
        var result = await mediatr.Send(command);

        return result.Match(
           success => Ok(success),
           Problem);
    }

    [HttpPost("send-request")]
    public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequest request)
    {
        var baseUrl = Request.Headers.Referer.ToString();

        var command = new SendFriendRequestCommand(request.FriendId, baseUrl);
        var result = await mediatr.Send(command);

        return result.Match(
            success => Ok(success),
            Problem);
    }

    [HttpPost("reject-request")]
    public async Task<IActionResult> RejectFriendRequest([FromBody] FriendRequest request)
    {
        var command = new RejectFriendRequestCommand(request.FriendId);
        var result = await mediatr.Send(command);

        return result.Match(
           success => Ok(success),
           Problem);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveFriend([FromBody] FriendRequest request)
    {
        var command = new RemoveFriendCommand(request.FriendId);
        var result = await mediatr.Send(command);

        return result.Match(
           success => Ok(success),
           Problem);
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
        catch (Exception)
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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while getting friend.");
        }
    }

    [HttpPost("search-friends-by-first-and-last-names")]
    public async Task<IActionResult> SearchFriendsByFirstAndLastNames(SearchUsersByFirstAndLastNamesRequest request)
    {
        var result = await mediatr.Send(mapper.Map<SearchByFirstAndLastNamesQuery>(request));

        if (result.IsError)
        {
            return BadRequest(result.Errors);
        }

        var users = result.Value.Adapt<List<UserDto>>();

        return Ok(users);
    }

    [HttpGet("recommendations")]
    public async Task<IActionResult> GetFriendsRecommendations()
    {
        var query = new GetFriendsRecommendationsQuery();
        var result = await mediatr.Send(query);

        return result.Match(
           success => Ok(success),
           error => Problem(error));
    }

    [HttpGet("requests")]
    public async Task<IActionResult> GetAllFriendRequests([FromQuery] GetAllFriendRequestsRequest request)
    {
        var query = new GetAllFriendRequestsQuery(request.UserId);
        var result = await mediatr.Send(query);

        return result.Match(
           success => Ok(success),
           error => Problem(error));
    }

    [HttpGet("relationships-status")]
    public async Task<IActionResult> GetAllFriendRequests([FromQuery] Guid friendId)
    {
        var query = new GetRelationshipsStatusQuery(friendId.ToString());
        var result = await mediatr.Send(query);

        return result.Match(
           success => Ok(result.Value),
           Problem);
    }
}