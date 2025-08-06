using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmpGradeProfile'
    public class EmpGradeProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmpGradeProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmpGradeProfile.EmpGradeProfile()'
        public EmpGradeProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmpGradeProfile.EmpGradeProfile()'
        {
            CreateMap<EmpGrade, EmpGradeDto>().ReverseMap();
            CreateMap<AddEmpGradeCommand, EmpGrade>();
            CreateMap<UpdateEmpGradeCommand, EmpGrade>();
            // CreateMap<DeleteGradeCommand, Grade>();
        }
    }
}
