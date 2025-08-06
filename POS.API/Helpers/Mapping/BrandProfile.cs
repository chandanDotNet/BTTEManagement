using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Brand.Command;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BrandProfile'
    public class BrandProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BrandProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BrandProfile.BrandProfile()'
        public BrandProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BrandProfile.BrandProfile()'
        {
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<AddBrandCommand, Brand>();
        }
    }
}
