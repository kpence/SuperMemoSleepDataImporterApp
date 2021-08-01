using System;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using SleepDataImporter;
using SleepDataImporter.Models;

namespace FitbitSuperMemoSleepDataImporterApp
{
	public class Program
    {
        static void Main(string[] args)
        {
            var input = InputHelper.GetInputResponseFromUserPrompts();

            if (input.OverwriteBehavior == "Abort")
            {
                System.Environment.Exit(0);
            }

            // Authorize with FitBit
            FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("activity ", "nutrition ", "heartrate ", "location ", "nutrition ", "profile ", "settings ", "sleep ", "social ", "weight");
            //FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("sleep ");

            // Let's get last weeks data
            var sleepReg = new SleepDataRegistry(input.RegistryPath);
            DateTime startDate = input.StartDate ?? new DateTime();
            DateTime endDate = input.EndDate ?? new DateTime();

            SleepBlock[] sleepBlocks = SleepBlockRepository.FetchSleepBlocksInDateRange(fc, startDate, endDate);

            if (input.OverwriteBehavior == "MergePickExisting" || input.OverwriteBehavior == "MergePickNew")
            {
                SleepBlock[] existingSleepBlocks = SleepBlockRepository.FetchSleepBlocksFromFile(sleepReg);
                string overwriteOpt = input.OverwriteBehavior;
                var overwriteStrategy = (overwriteOpt == "MergePickNew")
                    ? (SleepBlockMergeStrategy)(new PickNewStrategy())
                    : new PickExistingStrategy();
                var merger = new SleepBlockListMerger(overwriteStrategy);
                sleepBlocks = merger.Merge(existingSleepBlocks, sleepBlocks);
            }

            // Print the results (show it's working!)
            foreach (var block in sleepBlocks)
            {
                Console.WriteLine(block.ToString());
            }
            if (sleepReg.WriteSleepData(new List<SleepBlock>(sleepBlocks))) 
            {
                Console.WriteLine("Successfully wrote data to {0}!", input.RegistryPath);
            }
            else
            {
                Console.WriteLine("Failed to write to file!");
            }
        }
    }
}
