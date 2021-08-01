using System;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using Fitbit.Models;
using SleepDataImporter;
using SleepDataImporter.Models;

namespace FitbitSuperMemoSleepDataImporterApp
{
	public class Program
    {
        static void Main(string[] args)
        {
            var input = InputHelper.GetInputResponseFromUserPrompts();

            //var file = @"C:\Users\james\SuperMemo\sleep\sleep.tim";
            //var sleepReg = new SleepDataRegistry(file);
            //sleepReg.ReadSleepData();

            // Authorize with FitBit
            FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("activity ", "nutrition ", "heartrate ", "location ", "nutrition ", "profile ", "settings ", "sleep ", "social ", "weight");
            //FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("sleep ");

            // Let's get last weeks data
            DateTime startDate = DateTime.Now - TimeSpan.FromDays(7);
            DateTime endDate = DateTime.Now;
            SleepBlock[] sleepBlocks = SleepBlockRepository.fetchSleepBlocksInDateRange(fc, startDate, endDate);

            // Print the results (show it's working!)
            foreach (var block in sleepBlocks)
            {
                Console.WriteLine(block.ToString());
            }
        }
    }
}
