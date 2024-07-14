namespace Facebook.Contracts.UserProfile.Delete;

public record DeleteAvatarCoverPhotoRequest
{
    public Guid UserId { get; set; }
}