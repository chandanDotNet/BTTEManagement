using AutoMapper;
using POS.Data.Dto;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NLogProfile'
    public class NLogProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NLogProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NLogProfile.NLogProfile()'
        public NLogProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NLogProfile.NLogProfile()'
        {
            CreateMap<Data.NLog, NLogDto>().ReverseMap();
        }
    }
}
