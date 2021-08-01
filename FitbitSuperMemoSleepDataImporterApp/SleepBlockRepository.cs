using System;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Models;
using SleepDataImporter.Models;
using SleepDataImporter;

namespace FitbitSuperMemoSleepDataImporterApp
{
    class SleepBlockRepository
    {
        public static SleepBlock[] FetchSleepBlocksInDateRange(FitbitClient fc, DateTime startDate, DateTime endDate)
        {
            SleepDateRangeBase sleepDateRange = fc.GetSleepDateRangeAsync(startDate, endDate).Result;

            var sleepBlocks = new List<SleepBlock>();
            foreach (SleepLogDateRange sleepData in sleepDateRange.Sleep)
            {
                sleepBlocks.Add(SleepBlockMapper.FromSleepLogDateRange(sleepData));
            }
            return sleepBlocks.ToArray();
        }
        public static SleepBlock[] FetchSleepBlocksFromFile(SleepDataRegistry sleepReg)
        {
            var sleepBlocks = sleepReg.ReadSleepData();
            return sleepBlocks.ToArray();
        }
    }
}
