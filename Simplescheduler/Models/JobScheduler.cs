using Quartz;
using Quartz.Impl;

namespace Simplescheduler.Models
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler sched =  StdSchedulerFactory.GetDefaultScheduler();
            sched.Start();

            IJobDetail notificationScheduler = JobBuilder.Create<SchedulerProcess>()
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
               .WithSimpleSchedule(x => x
              .WithIntervalInHours(1)
              .RepeatForever()
           )
           .StartNow()
           .Build();

            sched.ScheduleJob(notificationScheduler, trigger);
        }
    
    }
}