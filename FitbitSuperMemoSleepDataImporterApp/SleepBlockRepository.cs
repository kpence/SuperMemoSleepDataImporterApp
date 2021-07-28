using System;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Models;
using SleepDataImporter.Models;

namespace FitbitSuperMemoSleepDataImporterApp
{
    class SleepBlockRepository
    {
        public static SleepBlock[] fetchSleepBlocksInDateRange(FitbitClient fc, DateTime startDate, DateTime endDate)
        {
            SleepDateRangeBase sleepDateRange = fc.GetSleepDateRangeAsync(startDate, endDate).Result;

            var sleepBlocks = new List<SleepBlock>();
            foreach (SleepLogDateRange sleepData in sleepDateRange.Sleep)
            {
                sleepBlocks.Add(SleepBlockMapper.FromSleepLogDateRange(sleepData));
            }
            return sleepBlocks.ToArray();
        }
    }
}
