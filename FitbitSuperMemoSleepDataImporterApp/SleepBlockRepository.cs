using System;
using System.Linq;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Models;
using SleepDataImporter;

namespace FitbitSuperMemoSleepDataImporterApp
{
    class SleepBlockRepository
    {
        public static ISleepBlockAdapter[] FetchSleepBlocksInDateRange(FitbitClient fc, DateTime startDate, DateTime endDate)
        {
            SleepDateRangeBase sleepDateRange = fc.GetSleepDateRangeAsync(startDate, endDate).Result;

            var sleepBlocks = new List<ISleepBlockAdapter>();
            foreach (SleepLogDateRange sleepData in sleepDateRange.Sleep)
            {
                sleepBlocks.Add(SleepBlockMapper.FromSleepLogDateRange(sleepData));
            }
            return sleepBlocks.ToArray();
        }
        public static ISleepBlockAdapter[] FetchSleepBlocksFromFile(SleepDataRegistry sleepReg)
        {
            var libSleepBlocks = sleepReg.ReadSleepData();
            var sleepBlocks = libSleepBlocks.Select(block => SleepBlockAdapter.FromSleepBlock(block));
            return sleepBlocks.ToArray();
        }
    }
}
