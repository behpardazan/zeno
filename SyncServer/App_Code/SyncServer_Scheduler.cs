using DataLayer.Schedules;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncServer
{
    public class SyncServer_Scheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            /*
            //  EmailInboxScheduler(scheduler);
            //  TelegramScheduler(scheduler);            
            */
            ProductScheduler(scheduler);
            ProductTypeScheduler(scheduler);
            ProductBrandScheduler(scheduler);
            ProductCategoryScheduler(scheduler);
            ProductSubCategoryScheduler(scheduler);
            ProductCustomFieldScheduler(scheduler);
            SmsScheduler(scheduler);
            EmailScheduler(scheduler);
            ShrinkDatabaseScheduler(scheduler);
            AccountBasketScheduler(scheduler);
            AlexaScheduler(scheduler);
            GoldPriceScheduler(scheduler);
            PaymentSubscriptionScheduler(scheduler);
            LadderScheduler(scheduler);

            DiscountScheduler(scheduler);
        }
        public static void DiscountScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Discount>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                        .RepeatForever())
                .StartNow()
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
        public static void LadderScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Ladder>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(24)
                        .RepeatForever())
                .StartNow()
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }

        public static void AlexaScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Alexa>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void AccountBasketScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_AccountBasket>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ShrinkDatabaseScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_ShrinkDatabase>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(24)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void TelegramScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Telegram>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInSeconds(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void EmailScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Email>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(2)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void SmsScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Sms>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInSeconds(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ProductScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Product>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInSeconds(30)
                    .RepeatForever())
                .StartNow()
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }

        public static void ProductSubCategoryScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_ProductSubCategory>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
        public static void PaymentSubscriptionScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_PaymentSubscription>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(24)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ProductCategoryScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_ProductCategory>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ProductTypeScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_ProductType>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ProductBrandScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_ProductBrand>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void ProductCustomFieldScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_ProductCustomField>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void EmailInboxScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_EmailInbox>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
        public static void GoldPriceScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_GoldPrice>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(30)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}