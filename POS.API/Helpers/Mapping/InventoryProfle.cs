using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Inventory.Command;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InventoryProfle'
public class InventoryProfle : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InventoryProfle'
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InventoryProfle.InventoryProfle()'
    public InventoryProfle()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InventoryProfle.InventoryProfle()'
    {
        CreateMap<Inventory, InventoryDto>().ReverseMap();
        CreateMap<InventoryHistory, InventoryHistoryDto>();
        CreateMap<AddInventoryCommand, InventoryDto>();
    }
}
