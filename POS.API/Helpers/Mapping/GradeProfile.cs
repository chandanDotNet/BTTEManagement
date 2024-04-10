using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.City.Commands;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class GradeProfile : Profile
    {
        public GradeProfile()
        {
            CreateMap<Grade, GradeDto>().ReverseMap();
            CreateMap<AddGradeCommand, Grade>();
            CreateMap<UpdateGradeCommand, Grade>();
           // CreateMap<DeleteGradeCommand, Grade>();
        }

    }
}
