using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Warehouse.Commands;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'WarehouseProfile'
    public class WarehouseProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'WarehouseProfile'
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProfile()
        {
            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
            CreateMap<AddWarehouseCommand, Warehouse>();
            CreateMap<UpdateWarehouseCommand, Warehouse>();
        }
    }
}
