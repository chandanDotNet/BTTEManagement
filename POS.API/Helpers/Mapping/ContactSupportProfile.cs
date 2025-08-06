using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactSupportProfile'
    public class ContactSupportProfile :Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactSupportProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactSupportProfile.ContactSupportProfile()'
        public ContactSupportProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactSupportProfile.ContactSupportProfile()'
        {
            CreateMap<ContactSupport, ContactSupportDto>().ReverseMap();
            CreateMap<AddContactSupportCommand, ContactSupport>();
        }
    }
}
