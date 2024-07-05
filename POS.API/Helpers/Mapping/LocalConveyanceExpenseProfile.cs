using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.CommandAndQuery;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data.Entities;

namespace BTTEM.API.Helpers.Mapping
{
    public class LocalConveyanceExpenseProfile : Profile
    {

        public LocalConveyanceExpenseProfile()
        {

            CreateMap<LocalConveyanceExpense, LocalConveyanceExpenseDto>().ReverseMap();
            CreateMap<LocalConveyanceExpenseDocument, LocalConveyanceExpenseDocumentDto>().ReverseMap();
            CreateMap<AddLocalConveyanceExpenseCommand, LocalConveyanceExpense>();
            CreateMap<AddLocalConveyanceExpenseDocumentCommand, LocalConveyanceExpenseDocument>();
        }

    }
}
