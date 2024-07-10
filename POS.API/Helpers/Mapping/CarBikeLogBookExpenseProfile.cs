using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class CarBikeLogBookExpenseProfile : Profile
    {

        public CarBikeLogBookExpenseProfile()
        {
            CreateMap<CarBikeLogBookExpense, CarBikeLogBookExpenseDto>().ReverseMap();
            CreateMap<CarBikeLogBookExpenseDocument, CarBikeLogBookExpenseDocumentDto>().ReverseMap();
            CreateMap<AddCarBikeLogBookExpenseCommand, CarBikeLogBookExpense>();
            CreateMap<UpdateCarBikeLogBookExpenseCommand, CarBikeLogBookExpense>();
            CreateMap<AddCarBikeLogBookExpenseDocumentCommand, CarBikeLogBookExpenseDocument>();
        }
    }
}
