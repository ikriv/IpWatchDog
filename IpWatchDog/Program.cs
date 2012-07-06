using System;
using System.ServiceProcess;
using IpWatchDog.Runners;
using IpWatchDog.Log;
using IpWatchDog.Install;

namespace IpWatchDog
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("This is a service. Run with -h switch to see usage");
                RunAsService();
                return;
            }

            switch (args[0])
            {
                case "-c":
                case "-console":
                    RunAsConsole();
                    break;

                case "-i":
                case "-install":
                    Install(false);
                    break;

                case "-u":
                case "-uninstall":
                    Install(true);
                    break;

                case "-s":
                case "-start":
                    StartService(true);
                    break;

                case "-p":
                case "-stop":
                    StartService(false);
                    break;

                default:
                    Usage();
                    break;
            }
        }

        private static void RunAsService()
        {
            var log = new SystemLog(StringConstants.ServiceName);
            var service  = new Configurator(log).CreateWatchDogService();
            new ServiceRunner(service, StringConstants.ServiceName).Run();
        }

        private static void RunAsConsole()
        {
            var log = new ConsoleLog();
            var service = new Configurator(log).CreateWatchDogService();
            new ConsoleRunner(service).Run();
        }

        private static void Install(bool undo)
        {
            new InstallUtil().Install(undo);
        }

        private static void StartService(bool start)
        {

            try
            {
                Console.Write(start ? "Starting service... " : "Stopping service... ");
                var controller = new ServiceController(StringConstants.ServiceName);
                const int timeout = 10000;
                var targetStatus = start ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped;

                if (start)
                {
                    controller.Start();
                }
                else
                {
                    controller.Stop();
                }

                controller.WaitForStatus(targetStatus, TimeSpan.FromMilliseconds(timeout));

                if (controller.Status != targetStatus)
                {
                    Console.WriteLine("Failed");
                }
                else
                {
                    Console.WriteLine("Success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex);
            }
        }

        private static void Usage()
        {
            Console.WriteLine(@"IP Watchdog service: monitors computer's external IP and sends e-mail when it changes.

Command line arguments:
    -i or -install : install the service (must be an administrator)
    -u or -uninstall: uninstall the service (must be an administrator)
    -s or -start: start the service (must be an administrator)
    -p or -stop: stop running service (must be an administrator)
    -c or -console: run in console mode");
        }
    }
}
