using AutoMapper;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryActivityProfile'
    public class InquiryActivityProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryActivityProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryActivityProfile.InquiryActivityProfile()'
        public InquiryActivityProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryActivityProfile.InquiryActivityProfile()'
        {
            CreateMap<InquiryActivity, InquiryActivityDto>().ReverseMap();
            CreateMap<AddInquiryActivityCommand, InquiryActivity>();
            CreateMap<UpdateInquiryActivityCommand, InquiryActivity>();
        }
    }
}
