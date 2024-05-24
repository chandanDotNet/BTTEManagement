using AutoMapper;
using BTTEM.Data.Dto;
using BTTEM.Data;
using BTTEM.MediatR.CommandAndQuery;

namespace BTTEM.API.Helpers.Mapping
{
    public class ContactSupportProfile :Profile
    {
        public ContactSupportProfile()
        {
            CreateMap<ContactSupport, ContactSupportDto>().ReverseMap();
            CreateMap<AddContactSupportCommand, ContactSupport>();
        }
    }
}
