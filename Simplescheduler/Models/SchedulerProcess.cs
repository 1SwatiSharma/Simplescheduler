using Quartz;
using System;
using System.IO;
using System.Text;
using Simplescheduler.Logger;

namespace Simplescheduler.Models
{
    public class SchedulerProcess : IJob
    {
        public static DateTime nextStepStartTime { get; set; }

        public static int userId = 1;

        public void Execute(IJobExecutionContext context)
        {
            ILogger logger = new TextLogger($"FPI_{userId}");
            logger.Log("Scheduler Process start.Time Taken = " + DateTime.UtcNow, LogType.Error);
            nextStepStartTime = DateTime.UtcNow;

            try
            {
                TextFile();
                logger.Log("Write Text File.Time Taken = " + DateTime.UtcNow, LogType.Error);
            }
            catch (Exception ex)
            {
                logger.Log("Scheduler Execute Exception.Time Taken = " + (DateTime.UtcNow - nextStepStartTime), LogType.Error, ex);
            }
        }

        public static void TextFile()
        {
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/TextFile");
            string fileName = filepath + @"\" + "NewFile.txt";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.AppendAllText(fileName, "New Text" + Environment.NewLine);
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    //Add some text to file
                    byte[] title = new UTF8Encoding(true).GetBytes("New Text File");
                    fs.Write(title, 0, title.Length);
                }
            }
        }


    }
}