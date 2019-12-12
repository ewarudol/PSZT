using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaskCore
{
    public class Timeline
    {
        public List<Task> TasksList { get; set; }

        public int CurrentTime { get; set; }
    }
    public class Schedule
    {
        public string FileName { get; set; }
        public int DownloadTime { get; set; }
        public int D { get; set; }
        public List<Timeline> TimeLinesList { get; set; }

        public Schedule(int machines = 4)
        {
            InitTimeLines(machines);
        }

        public Schedule(string file, int machines = 4)
        {
            InitTimeLines(machines);
            Parse(file);
        }

        private void InitTimeLines(int machines)
        {
            DownloadTime = -1;
            TimeLinesList = new List<Timeline>();

            for (int i = 0; i < machines; i++)
            {
                Timeline newTimeLine = new Timeline
                {
                    TasksList = new List<Task>(),
                    CurrentTime = 0
                };
                TimeLinesList.Add(newTimeLine);
            }
        }

        private void Parse(string file)
        {
            using (StringReader reader = new StringReader(file))
            {
                string line;
                int lineIterator = -1;

                while ((line = reader.ReadLine()) != null)
                {
                    if (lineIterator == -1)
                    {
                        int.TryParse(line, out int res);
                        D = res;
                    }
                    else
                    {
                        string[] parts = line.Split(' ');
                        foreach(var part in parts)
                        {
                            if (!string.IsNullOrEmpty(part))
                            {
                                int.TryParse(part, out int res);
                                TimeLinesList[lineIterator].TasksList.Add(new Task { Id = res });
                            }
                        }
                    }
                    lineIterator++;
                }
            }
        }

        public void ExportToFile(string path)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter($@"{path}"))
            {
                file.WriteLine(D);

                foreach (Timeline timeline in TimeLinesList)
                {
                    bool firstTask = true;
                    foreach(Task task in timeline.TasksList)
                    {
                        if (firstTask)
                            file.Write($"{task.Id}");
                        else
                            file.Write($" {task.Id}");

                        firstTask = false;
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
