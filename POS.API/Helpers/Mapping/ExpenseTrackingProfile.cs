using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class ExpenseTrackingProfile : Profile
    {
        public ExpenseTrackingProfile()
        {
            CreateMap<ExpenseTracking, ExpenseTrackingDto>().ReverseMap();
            CreateMap<AddExpenseTrackingCommand, ExpenseTracking>();
            //CreateMap<UpdateTripCommand, Trip>();
        }
    }
}