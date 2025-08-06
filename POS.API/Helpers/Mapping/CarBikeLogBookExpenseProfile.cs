using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data.Entities;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CarBikeLogBookExpenseProfile'
    public class CarBikeLogBookExpenseProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CarBikeLogBookExpenseProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'CarBikeLogBookExpenseProfile.CarBikeLogBookExpenseProfile()'
        public CarBikeLogBookExpenseProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'CarBikeLogBookExpenseProfile.CarBikeLogBookExpenseProfile()'
        {
            CreateMap<CarBikeLogBookExpense, CarBikeLogBookExpenseDto>().ReverseMap();
            CreateMap<CarBikeLogBookExpenseDocument, CarBikeLogBookExpenseDocumentDto>().ReverseMap();
            CreateMap<CarBikeLogBookExpenseRefillingDocument, CarBikeLogBookExpenseRefillingDocumentDto>().ReverseMap();
            CreateMap<CarBikeLogBookExpenseTollParkingDocument, CarBikeLogBookExpenseTollParkingDocumentDto>().ReverseMap();
            CreateMap<AddCarBikeLogBookExpenseCommand, CarBikeLogBookExpense>();
            CreateMap<UpdateCarBikeLogBookExpenseCommand, CarBikeLogBookExpense>();
            CreateMap<AddCarBikeLogBookExpenseDocumentCommand, CarBikeLogBookExpenseDocument>();
        }
    }
}
