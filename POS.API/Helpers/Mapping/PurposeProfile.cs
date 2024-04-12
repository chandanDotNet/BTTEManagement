using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;

namespace BTTEM.API.Helpers.Mapping
{
    public class PurposeProfile : Profile
    {

        public PurposeProfile()
        {
            CreateMap<Purpose, PurposeDto>().ReverseMap();
            
            // CreateMap<DeleteGradeCommand, Grade>();
        }
    }
}
