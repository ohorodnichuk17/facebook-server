using System.ComponentModel.DataAnnotations;

namespace Facebook.Contracts.UserProfile.GetById
{
    public record GetUserProfileByIdRequest
    {
        [Required(ErrorMessage = "{PropertyName} is required.")]
        public required Guid UserId { get; init; }
    }
}
