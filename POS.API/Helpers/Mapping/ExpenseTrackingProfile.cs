using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExpenseTrackingProfile'
    public class ExpenseTrackingProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExpenseTrackingProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ExpenseTrackingProfile.ExpenseTrackingProfile()'
        public ExpenseTrackingProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ExpenseTrackingProfile.ExpenseTrackingProfile()'
        {
            CreateMap<ExpenseTracking, ExpenseTrackingDto>().ReverseMap();
            CreateMap<AddExpenseTrackingCommand, ExpenseTracking>();
            //CreateMap<UpdateTripCommand, Trip>();
        }
    }
}