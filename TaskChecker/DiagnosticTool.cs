using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskCore;

namespace TaskChecker
{
    public class DiagnosticTool
    {
        private void CollectScheduleInfo(Instance instance, Schedule schedule)
        {
            foreach(Timeline timeLine in schedule.TimeLinesList)
            {
                foreach(Task task in timeLine.TasksList)
                {
                    var sameTask = instance.TasksList.Where(t => t.Id == task.Id).FirstOrDefault();
                    task.D = sameTask.D;
                    task.P = sameTask.P;
                    task.R = sameTask.R;
                }
            }
            schedule.CalculateD();
        }

        public void ExportRaport(List<Instance> instancesList, List<Schedule> schedulesList, string fileName)
        {
            int packsCount = schedulesList.Count;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter($"{fileName}"))
            {
                for (int i = 0; i < packsCount; i++)
                {
                    var analyzingInstance = instancesList[i];
                    var analyzingSchedule = schedulesList[i];

                    var dParameterFromFile = analyzingSchedule.D;
                    CollectScheduleInfo(analyzingInstance, analyzingSchedule);
                    var dParameterCollectedInfo = analyzingSchedule.D;

                    string time = "";
                    if (analyzingSchedule.DownloadTime != -1)
                    {
                        time = analyzingSchedule.DownloadTime.ToString();
                    }
                    file.WriteLine($"{analyzingInstance.FileName};{analyzingSchedule.FileName};{dParameterFromFile};{dParameterCollectedInfo};{dParameterFromFile==dParameterCollectedInfo};{time}");
                }
            }
        }
    }
}
