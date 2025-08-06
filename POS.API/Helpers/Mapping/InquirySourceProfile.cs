using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceProfile'
    public class InquirySourceProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceProfile.InquirySourceProfile()'
        public InquirySourceProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceProfile.InquirySourceProfile()'
        {
            CreateMap<InquirySource, InquirySourceDto>().ReverseMap();
            CreateMap<AddInquirySourceCommand, InquirySource>();
            CreateMap<UpdateInquirySourceCommand, InquirySource>();
        }
    }
}
