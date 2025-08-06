using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data;
using BTTEM.Data.Dto;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TravelDeskProfile'
    public class TravelDeskProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TravelDeskProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'TravelDeskProfile.TravelDeskProfile()'
        public TravelDeskProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'TravelDeskProfile.TravelDeskProfile()'
        {
            CreateMap<TravelDeskExpense, TravelDeskExpenseDto>().ReverseMap();
            CreateMap<AddTravelDeskExpenseCommand, TravelDeskExpense>();
            CreateMap<UpdateTravelDeskExpenseCommand, TravelDeskExpense>();
        }
        
    }
}
