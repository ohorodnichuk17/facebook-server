using Facebook.Application.UserProfile.Command.DeleteAvatar;
using Facebook.Application.UserProfile.Command.DeleteCoverPhoto;
using Facebook.Application.UserProfile.Command.DeleteUser;
using Facebook.Application.UserProfile.Command.Edit;
using Facebook.Application.UserProfile.Query.GetById;
using Facebook.Application.UserProfile.Query.GetPostsByUserId;
using Facebook.Application.UserProfile.Query.GetStoriesByUserId;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.UserProfile.Delete;
using Facebook.Contracts.UserProfile.Edit;
using Facebook.Contracts.UserProfile.GetById;
using Facebook.Domain.Post;
using Facebook.Domain.Story;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/user-profile")]
[ApiController]
public class UserProfileController(ISender mediatr, IMapper mapper) : ApiController
{
    [HttpPut("edit-profile")]
    public async Task<IActionResult> EditProfileAsync([FromForm] UserEditProfileRequest request)
    {
        var coverPhoto = new byte[request.CoverPhoto == null ? 0 : request.CoverPhoto.Length];
        var avatar = new byte[request.Avatar == null ? 0 : request.Avatar.Length];

        if (request.CoverPhoto != null && request.CoverPhoto.Length != 0)
        {
            using MemoryStream memoryStream = new();
            await request.CoverPhoto.CopyToAsync(memoryStream);

            coverPhoto = memoryStream.ToArray();
        }
        if (request.Avatar != null && request.Avatar.Length != 0)
        {
            using MemoryStream memoryStream = new();
            await request.Avatar.CopyToAsync(memoryStream);

            avatar = memoryStream.ToArray();
        }

        var editResult = await mediatr.Send(
            mapper.Map<UserEditProfileCommand>((request, coverPhoto, avatar)));

        return editResult.Match(
            authResult => Ok(editResult.Value),
            errors => Problem(errors));
    }

    [HttpDelete("delete-profile")]
    public async Task<IActionResult> DeleteProfileAsync([FromBody] DeleteRequest request)
    {
        var command = mapper.Map<DeleteUserCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
        deleteRes => Ok(),
        errors => Problem(errors));
    }

    [HttpDelete("delete-avatar")]
    public async Task<IActionResult> DeleteAvatarAsync([FromQuery] DeleteAvatarCoverPhotoRequest request)
    {
        var command = mapper.Map<DeleteAvatarCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
            deleteRes => Ok(deleteResult.Value),
            errors => Problem(errors));
    }


    [HttpDelete("delete-cover-photo")]
    public async Task<IActionResult> DeleteCoverPhotoAsync([FromQuery] DeleteAvatarCoverPhotoRequest request)
    {
        var command = mapper.Map<DeleteCoverPhotoCommand>(request);
        var deleteResult = await mediatr.Send(command);

        return deleteResult.Match(
            deleteRes => Ok(),
            errors => Problem(errors));
    }

    [HttpGet("get-profile-by-id")]
    public async Task<IActionResult> GetUserProfileByIdAsync([FromQuery] GetUserProfileByIdRequest request)
    {
        var query = mapper.Map<GetUserProfileByIdQuery>(request);
        var getRes = await mediatr.Send(query);

        return getRes.Match(
        getRes => Ok(getRes),
        errors => Problem(errors));
    }

    [HttpGet("getPostsBy/{userId}")]
    public async Task<IActionResult> GetPostsByUserId(Guid userId)
    {
        try
        {
            var query = new GetPostsByUserIdQuery(userId);
            var posts = await mediatr.Send(query);

            var mappedPosts = posts.Value.Adapt<List<PostEntity>>();

            return Ok(mappedPosts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching posts.");
        }
    }

    [HttpGet("getStoriesBy/{userId}")]
    public async Task<IActionResult> GetStoriesByUserId(Guid userId)
    {
        try
        {
            var query = new GetStoriesByUserIdQuery(userId);
            var stories = await mediatr.Send(query);

            var mappedStories = stories.Value.Adapt<List<StoryEntity>>();

            return Ok(mappedStories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching stories.");
        }
    }
}