using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrder.Commands;
using POS.MediatR.PurchaseOrderPayment.Command;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PurchaseOrderProfile'
    public class PurchaseOrderProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PurchaseOrderProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PurchaseOrderProfile.PurchaseOrderProfile()'
        public PurchaseOrderProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PurchaseOrderProfile.PurchaseOrderProfile()'
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();
            CreateMap<AddPurchaseOrderCommand, PurchaseOrder>();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>().ReverseMap();
            CreateMap<PurchaseOrderItemTax, PurchaseOrderItemTaxDto>().ReverseMap();
            CreateMap<UpdatePurchaseOrderCommand, PurchaseOrder>();

            CreateMap<PurchaseOrderPayment, PurchaseOrderPaymentDto>().ReverseMap();
            CreateMap<AddPurchaseOrderPaymentCommand, PurchaseOrderPayment>();
            CreateMap<UpdatePurchaseOrderReturnCommand, PurchaseOrder>();
        }
    }
}
