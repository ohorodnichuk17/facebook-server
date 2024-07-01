using Facebook.Application.User.Friends.Command.AcceptFriendRequest;
using Facebook.Application.User.Friends.Command.RejectFriendRequest;
using Facebook.Application.User.Friends.Command.RemoveFriend;
using Facebook.Application.User.Friends.Command.SendFriendRequest;
using Facebook.Application.User.Friends.Query.SearchByFirstAndLastNames;
using Facebook.Contracts.Friends.AcceptFriendRequest;
using Facebook.Contracts.Friends.RejectFriendRequest;
using Facebook.Contracts.Friends.RemoveFriend;
using Facebook.Contracts.Friends.SendFriendRequest;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class FriendsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AcceptFriendRequestRequest, AcceptFriendRequestCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FriendRequestId, src => src.FriendRequestId);
        
        config.NewConfig<SendFriendRequestRequest, SendFriendRequestCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FriendId, src => src.FriendId);
        
        config.NewConfig<RejectFriendRequestRequest, RejectFriendRequestCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FriendRequestId, src => src.FriendRequestId);
        
        config.NewConfig<RemoveFriendRequest, RemoveFriendCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FriendId, src => src.FriendId);
    }
}