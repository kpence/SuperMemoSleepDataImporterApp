using System;
using Fitbit.Api.Portable.Models;
using SleepDataImporter.Models;

namespace FitbitSuperMemoSleepDataImporterApp
{
    class SleepBlockMapper
    {
        public static SleepBlock FromSleepLogDateRange(SleepLogDateRange sleepLogDateRange)
        {
            // TODO consider whether I should include the exact time we fall alseep, rather than just time in bed
            var start = sleepLogDateRange.StartTime;
            var duration = new TimeSpan(0, sleepLogDateRange.TimeInBed, 0);
            var end = start.Add(duration);
            var sleepBlock = new SleepBlock(start, end);
            return sleepBlock;
        }
    }
}
