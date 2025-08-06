using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailProfile'
    public class EmailProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailProfile.EmailProfile()'
        public EmailProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailProfile.EmailProfile()'
        {
            CreateMap<EmailSMTPSetting, EmailSMTPSettingDto>().ReverseMap();
            CreateMap<EmailSMTPSetting, AddEmailSMTPSettingCommand>().ReverseMap();
            CreateMap<EmailSMTPSetting, UpdateEmailSMTPSettingCommand>().ReverseMap();
        }
    }
}
