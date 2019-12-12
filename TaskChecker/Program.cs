using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TaskCore;

namespace TaskChecker
{
    class Program
    {
        static string dignosticFileName = @"diagnostic/raport.csv";
        static string downloadedSchedulName = "generated-schedule";

        static void Main(string[] args)
        {
            Console.WriteLine("Prepare data");
            var instancesFolderName = "instances";
            var schedulesFolderName = "schedules";
            var scriptsFolderName = "scheduleScripts";

            List<Instance> instancesList = new List<Instance>();
            List<Schedule> scheduleList = new List<Schedule>();

            Console.WriteLine($"Load instances from folder: {instancesFolderName}");
            foreach (string file in Directory.EnumerateFiles(instancesFolderName, "*"))
            {
                // ### 1.1. load instances
                string contents = File.ReadAllText(file);

                // ### 1.2. parse instance file into instance
                Instance newInstance = new Instance(contents)
                {
                    FileName = file
                };

                // ### 1.3. update instances list
                instancesList.Add(newInstance);
                Console.WriteLine($"Instance: {file} - added ");
            }

            //if there is one arg run #MODE 2 - download schedules first
            if (args.Length == 1)
            {
                string scriptName = args[0];
                Console.WriteLine($"Begin MODE 2, generate raport based on files from {instancesFolderName} and script name: {scriptName}");

                ScheduleDownloader downloader = new ScheduleDownloader();
                foreach (string file in Directory.EnumerateFiles(instancesFolderName, "*"))
                {
                    string generatedFileName = file.Replace("/", "-") + "_" + downloadedSchedulName;
                    string bashCommand = $"sh {scriptsFolderName}/{scriptName} {file} {schedulesFolderName}/{generatedFileName}";

                    Console.WriteLine($"Command execution: {bashCommand}");

                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    bashCommand.Bash(); //EXEC

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    int elapsedTime = ts.Milliseconds;

                    Console.WriteLine($"Schedule : {schedulesFolderName}/{downloadedSchedulName} - parsing");
                    //load schedule
                    string contents = File.ReadAllText($"{schedulesFolderName}/{generatedFileName}");

                    //parse schedule file into schedule
                    Schedule newSchedule = new Schedule(contents)
                    {
                        DownloadTime = elapsedTime,
                        FileName = generatedFileName
                     };

                    //update schedules list
                    scheduleList.Add(newSchedule);

                    Console.WriteLine($"Schedule : {schedulesFolderName}/{downloadedSchedulName} - added");
                }

            }
            else //#MODE 1
            {
                Console.WriteLine($"Begin MODE 1, generate raport based on files from {instancesFolderName} and {schedulesFolderName} folders");

                foreach (string file in Directory.EnumerateFiles(schedulesFolderName, "*"))
                {
                    // ### 2.1. load schedules
                    string contents = File.ReadAllText(file);

                    // ### 2.2 parse schedule file into schedule
                    Schedule newSchedule = new Schedule(contents)
                    {
                        FileName = file
                    };

                    // ### 2.3 update schedules list
                    scheduleList.Add(newSchedule);
                }
            }

            Console.WriteLine($"Creating raport");

            DiagnosticTool tool = new DiagnosticTool();
            tool.ExportRaport(instancesList, scheduleList, dignosticFileName);

            Console.WriteLine($"All done");
        }
    }
}
