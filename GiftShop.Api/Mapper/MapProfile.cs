using AutoMapper;
using GiftShop.Api.Models;
using GiftShop.Domain.Entities;

namespace GiftShop.Api.Mapper;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Product, ProductModel>();
        CreateMap<ProductModel, Product>();

        CreateMap<Brand, BrandModel>();
        CreateMap<BrandModel, Brand>();
    }
}
