using POS.MediatR.CommandAndQuery;
using POS.Repository;
using Hangfire;
using MediatR;
using System;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService'
    public class JobService
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService._mediator'
        private readonly IConnectionMappingRepository _connectionMappingRepository;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.JobService(IMediator, IConnectionMappingRepository)'
        public JobService(IMediator mediator,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.JobService(IMediator, IConnectionMappingRepository)'
            IConnectionMappingRepository connectionMappingRepository)
        {
            _mediator = mediator;
            _connectionMappingRepository = connectionMappingRepository;
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.StartScheduler()'
        public void StartScheduler()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.StartScheduler()'
        {
            // * * * * *
            // 1 2 3 4 5

            // field #   meaning        allowed values
            // -------   ------------   --------------
            //    1      minute         0-59
            //    2      hour           0-23
            //    3      day of month   1-31
            //    4      month          1-12 (or use names)
            //    5      day of week    0-7 (0 or 7 is Sun, or use names)


            //Daily Reminder
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => DailyReminder(), "0 0 * * *", TimeZoneInfo.Utc); // Every 24 hours
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //Weekly Reminder
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => WeeklyReminder(), "10 0 * * *", TimeZoneInfo.Utc); // Every 24 hours
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //Monthy Reminder
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => MonthyReminder(), "20 0 * * *", TimeZoneInfo.Utc); // Every 24 hours
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //Quarterly Reminder
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => QuarterlyReminder(), "30 0 * * *", TimeZoneInfo.Utc); // Every 24 hours
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //HalfYearly Reminder
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => HalfYearlyReminder(), "40 0 * * *", TimeZoneInfo.Utc); // Every 24 hours
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //Yearly Reminder                                                                                
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => YearlyReminder(), "50 0 * * *", TimeZoneInfo.Utc); // Every 24 hours
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //Customer Date
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => CustomDateReminderSchedule(), "59 0 * * *", TimeZoneInfo.Utc); // Every 24 hours
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //Reminder Scheduler To Send Email
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => ReminderSchedule(), "*/10 * * * *", TimeZoneInfo.Utc); // Every 10 minutes
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            //Send Email Scheduler To Send Email
#pragma warning disable CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'
            RecurringJob.AddOrUpdate(() => SendEmailSuppliersSchedule(), "*/15 * * * *", TimeZoneInfo.Utc); // Every 10 minutes
#pragma warning restore CS0618 // 'RecurringJob.AddOrUpdate(Expression<Action>, string, TimeZoneInfo, string)' is obsolete: 'Please use an overload with the explicit recurringJobId parameter and RecurringJobOptions instead. Will be removed in 2.0.0.'

            ////Expected Delivery For Purchase Order
            //RecurringJob.AddOrUpdate(() => ExpectedDeliveryForPurchaseOrder(), "5 0 * * *", TimeZoneInfo.Local); // Every 24 hours

            ////Expected Delivery For Sales Order
            //RecurringJob.AddOrUpdate(() => ExpectedDeliveryForSaleOrder(), "15 0 * * *", TimeZoneInfo.Local); // Every 24 hours
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.DailyReminder()'
        public bool DailyReminder()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.DailyReminder()'
        {
            return _mediator.Send(new DailyReminderServicesQuery()).GetAwaiter().GetResult();
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.WeeklyReminder()'
        public bool WeeklyReminder()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.WeeklyReminder()'
        {
            return _mediator.Send(new WeeklyReminderServicesQuery()).GetAwaiter().GetResult();

        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.MonthyReminder()'
        public bool MonthyReminder()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.MonthyReminder()'
        {
            return _mediator.Send(new MonthlyReminderServicesQuery()).GetAwaiter().GetResult();
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.QuarterlyReminder()'
        public bool QuarterlyReminder()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.QuarterlyReminder()'
        {
            return _mediator.Send(new QuarterlyReminderServiceQuery()).GetAwaiter().GetResult();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.HalfYearlyReminder()'
        public bool HalfYearlyReminder()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.HalfYearlyReminder()'
        {
            return _mediator.Send(new HalfYearlyReminderServiceQuery()).GetAwaiter().GetResult();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.YearlyReminder()'
        public bool YearlyReminder()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.YearlyReminder()'
        {
            return _mediator.Send(new YearlyReminderServicesQuery()).GetAwaiter().GetResult();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.ReminderSchedule()'
        public bool ReminderSchedule()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.ReminderSchedule()'
        {
            var schedulerStatus = _connectionMappingRepository.GetSchedulerServiceStatus();
            if (!schedulerStatus)
            {
                _connectionMappingRepository.SetSchedulerServiceStatus(true);
                var result = _mediator.Send(new ReminderSchedulerServiceQuery()).GetAwaiter().GetResult();
                _connectionMappingRepository.SetSchedulerServiceStatus(false);
                return result;
            }
            return true;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.CustomDateReminderSchedule()'
        public bool CustomDateReminderSchedule()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.CustomDateReminderSchedule()'
        {
            return _mediator.Send(new CustomDateReminderServicesQuery()).GetAwaiter().GetResult();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JobService.SendEmailSuppliersSchedule()'
        public bool SendEmailSuppliersSchedule()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JobService.SendEmailSuppliersSchedule()'
        {
            var schedulerStatus = _connectionMappingRepository.GetEmailSchedulerStatus();
            if (!schedulerStatus)
            {
                _connectionMappingRepository.SetEmailSchedulerStatus(true);
                var result = _mediator.Send(new SendEmailSchedulerSupplierCommand()).GetAwaiter().GetResult();
                _connectionMappingRepository.SetEmailSchedulerStatus(false);
                return result;
            }
            return true;
        }
    }
}
