using AutoMapper;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.Grade.Commands;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PurposeProfile'
    public class PurposeProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PurposeProfile'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PurposeProfile.PurposeProfile()'
        public PurposeProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PurposeProfile.PurposeProfile()'
        {
            CreateMap<Purpose, PurposeDto>().ReverseMap();
            
            // CreateMap<DeleteGradeCommand, Grade>();
        }
    }
}
