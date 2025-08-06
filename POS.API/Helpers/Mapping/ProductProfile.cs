using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Product.Command;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ProductProfile'
    public class ProductProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ProductProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ProductProfile.ProductProfile()'
        public ProductProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ProductProfile.ProductProfile()'
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductTax, ProductTaxDto>().ReverseMap();
            CreateMap<AddProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
        }
    }
}
