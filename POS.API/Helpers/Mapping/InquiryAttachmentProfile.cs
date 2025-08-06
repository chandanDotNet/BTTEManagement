using AutoMapper;
using POS.Data.Dto;
using POS.Data.Entities;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryAttachmentProfile'
    public class InquiryAttachmentProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryAttachmentProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryAttachmentProfile.InquiryAttachmentProfile()'
        public InquiryAttachmentProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryAttachmentProfile.InquiryAttachmentProfile()'
        {
            CreateMap<InquiryAttachmentDto, InquiryAttachment>().ReverseMap();
        }
    }
}
