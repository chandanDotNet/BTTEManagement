using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;

namespace POS.API.Helpers.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReminderProfile'
    public class ReminderProfile : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReminderProfile'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReminderProfile.ReminderProfile()'
        public ReminderProfile()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReminderProfile.ReminderProfile()'
        {
            CreateMap<Reminder, ReminderDto>().ReverseMap();
            CreateMap<AddReminderCommand, Reminder>();
            CreateMap<UpdateReminderCommand, Reminder>();
            CreateMap<ReminderNotification, ReminderNotificationDto>().ReverseMap();
            CreateMap<ReminderUser, ReminderUserDto>().ReverseMap();
            CreateMap<DailyReminder, DailyReminderDto>().ReverseMap();
            CreateMap<QuarterlyReminder, QuarterlyReminderDto>().ReverseMap();
            CreateMap<HalfYearlyReminder, HalfYearlyReminderDto>().ReverseMap();
            CreateMap<ReminderScheduler, ReminderSchedulerDto>().ReverseMap();
        }
    }
}
