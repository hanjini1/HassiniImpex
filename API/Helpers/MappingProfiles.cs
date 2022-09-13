using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entites;
using Core.Entites.Identity;

namespace API.Helpers
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Product, ProductToReturnDto>()
      .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
      .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
      .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>()).ReverseMap();

      CreateMap<Address, AddressDto>().ReverseMap();
      CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
      CreateMap<BasketItem, BasketItemDto>().ReverseMap();
    }
  }
}