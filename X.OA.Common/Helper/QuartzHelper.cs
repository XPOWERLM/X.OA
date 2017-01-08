using Quartz;
using Quartz.Impl;
using System;
using System.ComponentModel;

namespace X.OA.Common.Helper
{
    public static class QuartzHelper
    {
        #region Create required objects

        static Action _action = null;
        static IScheduler scheduler = null;
        #endregion

        //[Description("Quartz")]
        //public static void StartPolling<TJob>() where TJob : class, IJob, new()
        //{
        //    // Grab the Scheduler instance from the factory
        //    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

        //    // Start Scheduler
        //    scheduler.Start();

        //    // Define the job and tie it to Job Class
        //    IJobDetail job = JobBuilder.Create<TJob>()
        //                    .WithIdentity("job1", "group1")
        //                    .Build();

        //    // Trigger the job to run now , and then repeat every 10 seconds
        //    ITrigger trigger = TriggerBuilder.Create().
        //                       WithIdentity("trigger1", "group1").
        //                       StartNow().
        //                       WithSimpleSchedule(x => x.
        //                            WithIntervalInHours(24).
        //                            RepeatForever()).
        //                       Build();

        //    // Schedule the job with trigger
        //    scheduler.ScheduleJob(job, trigger);

        //    //Thread.Sleep(20000);

        //    //scheduler.Shutdown();
        //}
        [Description("Quartz start")]
        public static void StartPolling(Action action)
        {
            _action = action;

            // Grab the Scheduler instance from the factory
            scheduler = StdSchedulerFactory.GetDefaultScheduler();

            // Start Scheduler
            scheduler.Start();

            // Define the job and tie it to Job Class
            IJobDetail job = JobBuilder.Create<SearchStatistic>()
                            .WithIdentity("job1", "group1")
                            .Build();

            // Config file is better
            // Trigger the job to run now , and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create().
                               WithIdentity("trigger1", "group1").
                               StartNow().
                               WithSimpleSchedule(x => x.
                                    WithIntervalInHours(24).
                                    RepeatForever()).
                               Build();

            // Schedule the job with trigger
            scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// Stop application
        /// </summary>
        [Description("Quartz stop")]
        public static void StopPolling()
        {
            scheduler?.Shutdown();
        }

        private class SearchStatistic : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                _action();
            }
        }
    }
}
