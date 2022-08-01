using Investment.Domain.DTOs;
using Investment.Domain.Entities;

namespace Investment.API.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<Asset, AssetReadDTO>()
                .ForMember(dest => dest.CodAtivo, opt => opt.MapFrom(src => src.AssetId))
                .ForMember(dest => dest.QtdeAtivo, opt => opt.MapFrom(src => (int)src.Volume))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Price)
                );

            CreateMap<UserAsset, CustomerAssetReadDTO>()
                .ForPath(dest => dest.CodAtivo, opt => opt.MapFrom(src => src.AssetId))
                .ForPath(dest => dest.CodCliente, opt => opt.MapFrom(src => src.UserId))
                .ForPath(dest => dest.QtdeAtivo, opt => opt.MapFrom(src => src.Quantity))
                .ForPath(dest => dest.Valor, opt => opt.MapFrom(src => src.Asset.Price * src.Quantity));
           
            CreateMap<Account, AccountReadDTO>()
                .ForMember(dest => dest.User, opt=> opt.MapFrom(src => new UserReadDTO()
                    {
                        FirstName = src.User.FirstName,
                        LastName = src.User.LastName,
                        PreferedName = src.User.PreferedName,
                        Cpf = src.User.Cpf,
                        InvestorStyle = src.User.InvestorStyle         
                    })
                );

        }
    }
}
