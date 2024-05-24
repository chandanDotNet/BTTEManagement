using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
    public class TravelDeskProfile :Profile
    {
        public TravelDeskProfile()
        {
            CreateMap<TravelDeskExpense, TravelDeskExpenseDto>().ReverseMap();
            CreateMap<AddTravelDeskExpenseCommand, TravelDeskExpense>();
            CreateMap<UpdateTravelDeskExpenseCommand, TravelDeskExpense>();
        }
        
    }
}
