using Facebook.Application.User.Friends.Command.AcceptFriendRequest;
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
        config.NewConfig<FriendRequest, AcceptFriendRequestCommand>();

        config.NewConfig<(AcceptFriendRequest request, string BaseUrl), SendFriendRequestCommand>()
            .Map(dest => dest.FriendId, src => src.request.FriendId)
            .Map(dest => dest.baseUrl, src => src.BaseUrl);

        config.NewConfig<FriendRequest, RemoveFriendCommand>();

        config.NewConfig<GetAllFriendRequestsRequest, GetAllFriendRequestsQuery>();
    }
}