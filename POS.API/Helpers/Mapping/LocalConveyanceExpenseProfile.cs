using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data.Entities;
using BTTEM.MediatR.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LocalConveyanceExpenseProfile'
    public class LocalConveyanceExpenseProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LocalConveyanceExpenseProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LocalConveyanceExpenseProfile.LocalConveyanceExpenseProfile()'
        public LocalConveyanceExpenseProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LocalConveyanceExpenseProfile.LocalConveyanceExpenseProfile()'
        {

            CreateMap<LocalConveyanceExpense, LocalConveyanceExpenseDto>().ReverseMap();
            CreateMap<LocalConveyanceExpenseDocument, LocalConveyanceExpenseDocumentDto>().ReverseMap();
            CreateMap<AddLocalConveyanceExpenseCommand, LocalConveyanceExpense>();
            CreateMap<UpdateLocalConveyanceExpenseCommand, LocalConveyanceExpense>();
            CreateMap<AddLocalConveyanceExpenseDocumentCommand, LocalConveyanceExpenseDocument>();
        }

    }
}
