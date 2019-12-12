using System;
using System.Collections.Generic;
using System.Text;

namespace TaskScheduler
{
    class Timeline
    {
        public List<Task> TasksList { get; set; }
    }
    class Schedule
    {
        public int D { get; set; }
        public List<Timeline> TimeLinesList { get; set; }

        public Schedule(int machines = 4)
        {
            TimeLinesList = new List<Timeline>();

            for (int i = 0; i<machines; i++)
            {
                Timeline newTimeLine = new Timeline
                {
                    TasksList = new List<Task>()
                };
                TimeLinesList.Add(newTimeLine);
            }
        }

        public void ExportToFile(string directory, string fileName)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter($@"{directory}\{fileName}"))
            {
                file.WriteLine(D);

                foreach (Timeline timeline in TimeLinesList)
                {
                    foreach(Task task in timeline.TasksList)
                    {
                        file.Write($"{task.Id} ");
                    }
                    file.Write(Environment.NewLine);
                }
            }
        }

        public void CalculateD()
        {
            D = 0;

            int currentTime = 0;

            foreach(Timeline timeline in TimeLinesList)
            {
                foreach(Task task in timeline.TasksList)
                {
                    var waitingTime = 0;

                    //if task isn't ready yet!
                    if (task.R > currentTime)
                        waitingTime = task.R - currentTime;
                    
                    currentTime += waitingTime + task.P;

                    //if there was tardiness
                    if (currentTime > task.D)
                        D += currentTime - task.D;
                }

                currentTime = 0;
            }

        }
    }
}
