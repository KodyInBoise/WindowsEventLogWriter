using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var parameters = ParseArguments(args);

                var _source = ScrubParam(parameters.Find(s => s.ToLower().Contains("source:"))) ?? "Event Log Writer";
                var _log = ScrubParam(parameters.Find(l => l.ToLower().Contains("log:"))) ?? "Application";
                var _event = ScrubParam(parameters.Find(e => e.ToLower().Contains("event:"))) ?? "Wrote to Windows Event Log";
                var _message = ScrubParam(parameters.Find(m => m.Contains("message:") || m.Contains("Message:"))) ?? "Empty Message";

                //Write to Windows event log
                if (!EventLog.SourceExists(_source)) EventLog.CreateEventSource(_source, _log);
                EventLog.WriteEntry(_source, _message, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine);
                Console.WriteLine("Press any key to exit!");
                Console.ReadKey();
            }
        }

        static List<string> ParseArguments(string[] args)
        {
            var argList = args.ToList();
            var parametersString = string.Empty;

            foreach (var arg in argList)
            {
                parametersString += $" {arg}";
            }

            var parameters = parametersString.Split('-').ToList();
            parameters.RemoveAt(0);

            return parameters;
        }

        static string ScrubParam(string param)
        {
            try
            {
                var paramList = param.Split(':').ToList();
                var paramValue = paramList[1].TrimStart().TrimEnd();

                return paramValue;
            }
            catch
            {
                return param;
            }
        }
    }
}
