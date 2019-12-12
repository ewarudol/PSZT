using System;
using System.Collections.Generic;
using System.IO;
using TaskCore;

namespace TaskScheduler
{
    class Program
    {
        private static readonly string instancesFolderName = @"D:\Zasoby\PROJECT LAB\PSZT\TaskScheduler\instances";
        private static readonly string schedulesFolderName = @"D:\Zasoby\PROJECT LAB\PSZT\TaskScheduler\schedules";

        private static string sourceInstancePath = "";
        private static string destinationSchedulePath = "";

        private static readonly string scheduleFileName = "schedule";

        static void Main(string[] args)
        {
            // ### 0. Root functionality

            //prepare
            ITaskScheduler scheduler = new SortScheduler();

            if(args.Length != 2)
            {
                Console.WriteLine("Received less than 2 arguments, aborting.");
                return;
            }

            while (args.Length == 2)
            {
                if (string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]))
                    break;

                sourceInstancePath = args[0];
                destinationSchedulePath = args[1];
                try
                {
                    // ### 1. load instances
                    string contents = File.ReadAllText(sourceInstancePath);

                    // ### 2. parse instance file into instance
                    Instance newInstance = new Instance(contents);

                    // ### 3. produce schedule
                    Schedule newSchedule = scheduler.GenerateSchedule(newInstance);

                    // ### 4. export schedule
                    newSchedule.CalculateD();
                    newSchedule.ExportToFile(destinationSchedulePath);
                }
                catch
                {
                    Console.WriteLine("File error, aborting.");
                    return;
                }

                Console.WriteLine("Success.");
                return;
            }

            //act
            foreach (string file in Directory.EnumerateFiles(instancesFolderName, "*"))
            {
                // ### 1. load instances
                string contents = File.ReadAllText(file);

                // ### 2. parse instance file into instance
                Instance newInstance = new Instance(contents);

                // ### 3. produce schedule
                Schedule newSchedule =  scheduler.GenerateSchedule(newInstance);

                // ### 4. export schedule
                newSchedule.CalculateD();
                newSchedule.ExportToFile($"{schedulesFolderName}/{scheduleFileName}-{newInstance.Size}.txt");
                Console.WriteLine("All done: Directories mode.");
            }

            Console.ReadKey();
        }
    }
}
