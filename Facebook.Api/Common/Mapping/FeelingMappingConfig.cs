using Facebook.Application.Feeling.Command.Add;
using Facebook.Application.Feeling.Command.Delete;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.Feeling.Add;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class FeelingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddFeelingRequest, AddFeelingCommand>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Emoji, src => src.Emoji);

        config.NewConfig<DeleteRequest, DeleteFeelingCommand>()
            .Map(dest => dest.Id, src => src.Id);
    }
}