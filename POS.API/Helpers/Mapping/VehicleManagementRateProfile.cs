using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class VehicleManagementRateProfile : Profile
    {
        public VehicleManagementRateProfile()
        {
            CreateMap<VehicleManagementRate, VehicleManagementRateDto>().ReverseMap();
            CreateMap<AddVehicleManagementRateCommand, VehicleManagementRate>();
            //CreateMap<UpdateVehicleManagementCommand, VehicleManagement>();
        }
    }
}