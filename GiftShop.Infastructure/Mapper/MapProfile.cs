using AutoMapper;
using GiftShop.Domain.Entities;
using GiftShop.Infastructure.DTOs;
using GiftShop.Infastructure.Models;

namespace GiftShop.Infastructure.Mapper;

public class MapProfile : Profile
{
    public MapProfile()
    {
        #region Product
        CreateMap<ProductDTO, Product>();
        CreateMap<Product, ProductDTO>();
        #endregion Product

        #region Brand
        CreateMap<Brand, BrandModel>();
        CreateMap<Brand, BrandDTO>();
        CreateMap<BrandModel, Brand>();
        #endregion Brand

        #region Product Property
        CreateMap<Property, PropertyDTO>();
        CreateMap<PropertyDTO, Property>();
        #endregion Product Property

        #region Product Image
        CreateMap<Image, ImageDTO>();
        CreateMap<ImageDTO, Image>();
        #endregion Product Image

        #region Category
        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryDTO, Category>();
        #endregion Category


    }
}
