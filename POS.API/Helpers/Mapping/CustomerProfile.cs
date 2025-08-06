using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CustomerProfile'
    public class CustomerProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CustomerProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CustomerProfile.CustomerProfile()'
        public CustomerProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CustomerProfile.CustomerProfile()'
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<AddCustomerCommand, Customer>();
            CreateMap<UpdateCustomerCommand, Customer>();
        }
    }
}
