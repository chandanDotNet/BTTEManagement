using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementRateProfile'
    public class VehicleManagementRateProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementRateProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementRateProfile.VehicleManagementRateProfile()'
        public VehicleManagementRateProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementRateProfile.VehicleManagementRateProfile()'
        {
            CreateMap<VehicleManagementRate, VehicleManagementRateDto>().ReverseMap();
            CreateMap<AddVehicleManagementRateCommand, VehicleManagementRate>();
            //CreateMap<UpdateVehicleManagementCommand, VehicleManagement>();
        }
    }
}