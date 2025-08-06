using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrder.Commands;
using POS.MediatR.SalesOrder.Commands;
using POS.MediatR.SalesOrderPayment.Command;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SalesOrderProfile'
    public class SalesOrderProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SalesOrderProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SalesOrderProfile.SalesOrderProfile()'
        public SalesOrderProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SalesOrderProfile.SalesOrderProfile()'
        {
            CreateMap<SalesOrder, SalesOrderDto>().ReverseMap();
            CreateMap<UpdateSalesOrderReturnCommand, SalesOrder>();
            CreateMap<AddSalesOrderCommand, SalesOrder>();
            CreateMap<SalesOrderItem, SalesOrderItemDto>().ReverseMap();
            CreateMap<SalesOrderItemTax, SalesOrderItemTaxDto>().ReverseMap();
            CreateMap<UpdateSalesOrderCommand, SalesOrder>();
            CreateMap<SalesOrderPayment,SalesOrderPaymentDto>().ReverseMap();
            CreateMap<AddSalesOrderPaymentCommand, SalesOrderPayment>();
        }
    }
}
