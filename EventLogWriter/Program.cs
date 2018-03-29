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

                var _source = parameters.Find(s => s.ToLower().Contains("source:"));
                var _log = parameters.Find(l => l.ToLower().Contains("log:"));
                var _event = parameters.Find(e => e.ToLower().Contains("event:"));
                var _message = parameters.Find(m => m.Contains("message:") || m.Contains("Message:"));

                //Scrub paramneters or set defaults
                if (!String.IsNullOrWhiteSpace(_source)) _source = ScrubParam(_source);
                else _source = "Event Log Writer";

                if (!String.IsNullOrWhiteSpace(_log)) _log = ScrubParam(_log);
                else _log = "Application";

                if (!String.IsNullOrWhiteSpace(_event)) _event = ScrubParam(_event);
                else _event = "Wrote to Windows Event Log";

                if (!String.IsNullOrWhiteSpace(_message)) _message = ScrubParam(_message);
                else _message = "Empty message";

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
