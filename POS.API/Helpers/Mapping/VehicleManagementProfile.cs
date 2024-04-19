using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class VehicleManagementProfile : Profile
    {
        public VehicleManagementProfile()
        {
            CreateMap<VehicleManagement, VehicleManagementDto>().ReverseMap();
            CreateMap<AddVehicleManagementCommand, VehicleManagement>();
            CreateMap<UpdateVehicleManagementCommand, VehicleManagement>();
        }
    }
}
