using System;
using System.Collections.Generic;
using System.Text;
using TaskCore;

namespace TaskScheduler
{
    class DummyScheduler : ITaskScheduler
    {
        public Schedule GenerateSchedule(Instance instance, int machines)
        {
            Schedule newSchedule = new Schedule(machines);

            double mathCeil = Math.Ceiling(instance.Size / (double)machines);
            int equalPartCount = (int)mathCeil;
            int equalPartIterator = 0;

            int currentTimeLineIterator = 0;
            var currentTimeLine = newSchedule.TimeLinesList[currentTimeLineIterator];

            for(int i=0; i<instance.Size; i++) 
            {
                currentTimeLine.TasksList.Add(instance.TasksList[i]);

                equalPartIterator++;

                if (equalPartIterator == equalPartCount && i+1!=instance.Size)
                {
                    equalPartIterator = 0;
                    currentTimeLineIterator++;
                    currentTimeLine = newSchedule.TimeLinesList[currentTimeLineIterator];
                }
            }

            return newSchedule;
        }
    }
}
