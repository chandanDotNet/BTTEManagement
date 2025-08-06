using AutoMapper;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryNoteProfile'
    public class InquiryNoteProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryNoteProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryNoteProfile.InquiryNoteProfile()'
        public InquiryNoteProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryNoteProfile.InquiryNoteProfile()'
        {
            CreateMap<AddInquiryNoteCommand, InquiryNote>();
            CreateMap<InquiryNoteDto, InquiryNote>().ReverseMap();
            CreateMap<UpdateInquiryNoteCommand, InquiryNote>();
        }
    }
}
