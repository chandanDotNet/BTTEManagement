using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementProfile'
    public class VehicleManagementProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementProfile.VehicleManagementProfile()'
        public VehicleManagementProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'VehicleManagementProfile.VehicleManagementProfile()'
        {
            CreateMap<VehicleManagement, VehicleManagementDto>().ReverseMap();
            CreateMap<AddVehicleManagementCommand, VehicleManagement>();
            CreateMap<UpdateVehicleManagementCommand, VehicleManagement>();
        }
    }
}
