using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PageProfile'
    public class PageProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PageProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'PageProfile.PageProfile()'
        public PageProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'PageProfile.PageProfile()'
        {
            CreateMap<Page, PageDto>().ReverseMap();
            CreateMap<AddPageCommand, Page>();
            CreateMap<UpdatePageCommand, Page>();
        }
    }
}
