using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Category.Commands;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ProductCategoryProfile'
    public class ProductCategoryProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ProductCategoryProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ProductCategoryProfile.ProductCategoryProfile()'
        public ProductCategoryProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ProductCategoryProfile.ProductCategoryProfile()'
        {
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<AddProductCategoryCommand, ProductCategory>();
            CreateMap<UpdateProductCategoryCommand, ProductCategory>();
        }
    }
}
