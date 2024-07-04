using Facebook.Application.UserProfile.Command.Delete;
using Facebook.Application.UserProfile.Command.Edit;
using Facebook.Application.UserProfile.Query.GetById;
using Facebook.Contracts.UserProfile.DeleteUser;
using Facebook.Contracts.UserProfile.Edit;
using Facebook.Contracts.UserProfile.GetUserProfileById;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Server.Controllers;

[Route("api/[controller]")]
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
         using MemoryStream memoryStream = new MemoryStream();
         await request.CoverPhoto.CopyToAsync(memoryStream);

         coverPhoto = memoryStream.ToArray();
      }
      if (request.Avatar != null && request.Avatar.Length != 0)
      {
         using MemoryStream memoryStream = new MemoryStream();
         await request.Avatar.CopyToAsync(memoryStream);

         avatar = memoryStream.ToArray();
      }

      var editResult = await mediatr.Send(
          mapper.Map<UserEditProfileCommand>((request, coverPhoto, avatar)));

      return editResult.Match(
          authResult => Ok(),
          errors => Problem(errors));
   }
   [HttpDelete("delete-profile")]
   public async Task<IActionResult> DeleteProfileAsync([FromForm] DeleteUserRequest request)
   {
      var command = mapper.Map<DeleteUserCommand>(request);
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
}
