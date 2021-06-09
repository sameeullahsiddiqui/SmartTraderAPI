using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmartTrader.Core.Helpers
{
    public static class Utilities
    {
        static ILoggerFactory _loggerFactory;


        public static void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }


        public static ILogger CreateLogger<T>()
        {
            if (_loggerFactory == null)
            {
                throw new InvalidOperationException($"{nameof(ILogger)} is not configured. {nameof(ConfigureLogger)} must be called before use");
            }

            return _loggerFactory.CreateLogger<T>();
        }


        public static void Log(string text, string filename)
        {
            string dirPath = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using (StreamWriter writer = File.AppendText(filename))
            {
                writer.WriteLine($"{DateTime.Now} - {text}");
            }
        }

        public static decimal CalculateChange(decimal previous, decimal current)
        {
            if (previous == 0)
                return 0;

            var change = current - previous;
            return Math.Round(((decimal)change / previous) * 100, 2);
        }


        public static DateTime PreviousWorkDay(DateTime date)
        {
            do
            {
                if (!IsWeekend(date))
                    return date;

                date = date.AddDays(-1);
            }
            while (IsHoliday(date) || IsWeekend(date));

            return date;
        }

        public static bool IsHoliday(DateTime date)
        {
            return false;
        }

        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday ||
                   date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
