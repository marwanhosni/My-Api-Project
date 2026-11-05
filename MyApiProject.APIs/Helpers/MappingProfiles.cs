using AutoMapper;
using MyApiProject.APIs.Dtos;
using MyApiProject.Core.Entities;
using MyApiProject.Core.Entities.Identity;
using UserAddress = MyApiProject.Core.Entities.Identity.Address;
using OrderAddress = MyApiProject.Core.Entities.Orders.Address;

namespace MyApiProject.APIs.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(D => D.Brand, O => O.MapFrom(S => S.Brand.Name))
            .ForMember(D => D.Category, O => O.MapFrom(S => S.Category.Name))
            .ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
       
        CreateMap<RegisterDto, AppUser>()
           .ForMember(
               destination => destination.UserName,
               options => options.MapFrom(source => source.Email.Split(
                new[] { '@' },
                StringSplitOptions.None)[0]));
        CreateMap<BasketItemDto, BasketItem>()
    .ReverseMap();

        CreateMap<CustmorBasketDto, CustomerBasket>()
            .ReverseMap();
        CreateMap<RegisterDto, AppUser>().ReverseMap();
        CreateMap<UserAddress, AddressDto>().ReverseMap();
        CreateMap<OrderAddress, AddressDto>().ReverseMap();

    }
}
