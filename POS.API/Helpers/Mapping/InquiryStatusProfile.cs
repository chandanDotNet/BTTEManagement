using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryStatusProfile'
    public class InquiryStatusProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryStatusProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryStatusProfile.InquiryStatusProfile()'
        public InquiryStatusProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryStatusProfile.InquiryStatusProfile()'
        {
            CreateMap<InquiryStatus, InquiryStatusDto>().ReverseMap();
            CreateMap<AddInquiryStatusCommand, InquiryStatus>();
        }
    }
}
