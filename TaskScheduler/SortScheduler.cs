using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskCore;

namespace TaskScheduler
{
    class SortScheduler : ITaskScheduler
    {
        public Schedule GenerateSchedule(Instance instance, int machines = 4)
        {
            Schedule schedule = new Schedule(machines);

            //sort by (R)eady time
            instance.TasksList = instance.TasksList.OrderBy(x => x.R).ToList();
            
            foreach(Task task in instance.TasksList)
            {
                //get timeline with the lowest current time
                var lowestTimeline = schedule.TimeLinesList.OrderBy(x => x.CurrentTime).FirstOrDefault();
                
                //add task
                lowestTimeline.TasksList.Add(task);
                
                //update timeline current time
                var waitingTime = 0;
                //if task isn't ready yet!
                if (task.R > lowestTimeline.CurrentTime)
                    waitingTime = task.R - lowestTimeline.CurrentTime;

                lowestTimeline.CurrentTime += waitingTime + task.P;
            }

            return schedule;
        }
    }
}
