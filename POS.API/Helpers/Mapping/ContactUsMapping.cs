using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactUsMapping'
    public class ContactUsMapping: Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactUsMapping'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactUsMapping.ContactUsMapping()'
        public ContactUsMapping()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactUsMapping.ContactUsMapping()'
        {
            CreateMap<ContactRequest, ContactUsDto>().ReverseMap();
            CreateMap<AddContactUsCommand, ContactRequest>();
        }
    }
}
