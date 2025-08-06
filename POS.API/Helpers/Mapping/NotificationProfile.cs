using AutoMapper;
using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using POS.Data.Dto;
using POS.Data;
using POS.MediatR.City.Commands;
using BTTEM.MediatR.Notification.Command;

namespace BTTEM.API.Helpers.Mapping
{
    public class NotificationProfile :Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>().ReverseMap();
            //CreateMap<Notification, NotificationDto>();
            CreateMap<AddNotificationCommand, Notification>();
            CreateMap<ReadNotificationCommand, Notification>();
        }
    }
}
