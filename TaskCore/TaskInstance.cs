using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaskCore
{
    public class Task
    {
        public int Id { get; set; }
        public int R { get; set; }
        public int P { get; set; }
        public int D { get; set; }
    }
    
    public class Instance
    {
        public string FileName { get; set; }
        public int Size { get; set; }
        public List<Task> TasksList { get; set; }

        public Instance(string file)
        {
            ParseFile(file);
        }
        private void ParseFile(string file)
        {
            Size = 0;
            TasksList = new List<Task>();

            using (StringReader reader = new StringReader(file))
            {
                string line;
                int taskIterator= 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (taskIterator == 0)
                    {
                        int.TryParse(line, out int res);
                        Size = res;
                    }
                    else
                    {
                        Task newTask = new Task();
                        string[] parts = line.Split(' ');
                        int.TryParse(parts[0], out int p);
                        int.TryParse(parts[1], out int r);
                        int.TryParse(parts[2], out int d);
                        newTask.P = p;
                        newTask.R = r;
                        newTask.D = d;
                        newTask.Id = taskIterator;
                        TasksList.Add(newTask);
                    }
                    taskIterator++;
                }
            }
        }
    }
}
