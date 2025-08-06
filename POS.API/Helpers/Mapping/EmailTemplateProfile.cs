using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailTemplateProfile'
    public class EmailTemplateProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailTemplateProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailTemplateProfile.EmailTemplateProfile()'
        public EmailTemplateProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailTemplateProfile.EmailTemplateProfile()'
        {
            CreateMap<EmailTemplateDto, EmailTemplate>().ReverseMap();
            CreateMap<AddEmailTemplateCommand, EmailTemplate>();
            CreateMap<UpdateEmailTemplateCommand, EmailTemplate>().ReverseMap();
        }
    }
}
