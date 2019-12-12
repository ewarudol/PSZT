using System;
using System.Collections.Generic;
using System.Text;
using TaskCore;

namespace TaskScheduler
{
    interface ITaskScheduler
    {
        public Schedule GenerateSchedule(Instance instance, int machines = 4);
    }
}
