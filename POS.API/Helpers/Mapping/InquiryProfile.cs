using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryProfile'
    public class InquiryProfile: Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryProfile.InquiryProfile()'
        public InquiryProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryProfile.InquiryProfile()'
        {
            CreateMap<InquiryProduct, InquiryProductDto>().ReverseMap();
            CreateMap<Inquiry, InquiryDto>().ReverseMap();
            CreateMap<AddInquiryCommand, Inquiry>().ReverseMap();
            CreateMap<UpdateInquiryCommand, Inquiry>().ReverseMap();
        }
    }
}
