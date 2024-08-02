using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.Command;

namespace BTTEM.API.Helpers.Mapping
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<AddVendorCommand, Vendor>();
            CreateMap<UpdateVendorCommand, Vendor>();
        }
    }
}
