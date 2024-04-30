using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class EmpGradeProfile : Profile
    {

        public EmpGradeProfile()
        {
            CreateMap<EmpGrade, EmpGradeDto>().ReverseMap();
            CreateMap<AddEmpGradeCommand, EmpGrade>();
            CreateMap<UpdateEmpGradeCommand, EmpGrade>();
            // CreateMap<DeleteGradeCommand, Grade>();
        }
    }
}
