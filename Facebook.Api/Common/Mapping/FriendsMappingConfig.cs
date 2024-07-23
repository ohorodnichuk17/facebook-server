using Facebook.Application.User.Friends.Command.AcceptFriendRequest;
using Facebook.Application.User.Friends.Command.RejectFriendRequest;
using Facebook.Application.User.Friends.Command.RemoveFriend;
using Facebook.Application.User.Friends.Command.SendFriendRequest;
using Facebook.Application.User.Friends.Query.GetAllFriendRequests;
using Facebook.Contracts.Friends;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class FriendsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FriendRequest, AcceptFriendRequestCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FriendId, src => src.FriendId);

        config.NewConfig<(FriendRequest request, string BaseUrl), SendFriendRequestCommand>()
            .Map(dest => dest.UserId, src => src.request.UserId)
            .Map(dest => dest.FriendId, src => src.request.FriendId)
            .Map(dest => dest.baseUrl, src => src.BaseUrl);

        config.NewConfig<FriendRequest, RejectFriendRequestCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FriendRequestId, src => src.FriendId);

        config.NewConfig<FriendRequest, RemoveFriendCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.FriendId, src => src.FriendId);

        config.NewConfig<GetAllFriendRequestsRequest, GetAllFriendRequestsQuery>();
    }
}