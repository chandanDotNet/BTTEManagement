using AutoMapper;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.City.Commands;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'GradeProfile'
    public class GradeProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'GradeProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'GradeProfile.GradeProfile()'
        public GradeProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'GradeProfile.GradeProfile()'
        {
            CreateMap<Grade, GradeDto>().ReverseMap();
            CreateMap<AddGradeCommand, Grade>();
            CreateMap<UpdateGradeCommand, Grade>();
           // CreateMap<DeleteGradeCommand, Grade>();
        }

    }
}
