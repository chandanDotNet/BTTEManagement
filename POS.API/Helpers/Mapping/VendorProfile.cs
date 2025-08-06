using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VendorProfile'
    public class VendorProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VendorProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VendorProfile.VendorProfile()'
        public VendorProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VendorProfile.VendorProfile()'
        {
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<AddVendorCommand, Vendor>();
            CreateMap<UpdateVendorCommand, Vendor>();
        }
    }
}
