using System;
using System.IO;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using SleepDataImporter;

namespace SuperMemoSleepDataImporterApp
{
	public class Program
    {
        static void Main(string[] args)
        {
            // Authorize with FitBit
            FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("activity ", "nutrition ", "heartrate ", "location ", "nutrition ", "profile ", "settings ", "sleep ", "social ", "weight");
            //FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("sleep ");

            var input = InputHelper.GetInputResponseFromUserPrompts();


            if (input.OverwriteBehavior == "Abort")
            {
                Environment.Exit(0);
            }

            // Let's get last weeks data
            DateTime startDate = input.StartDate ?? new DateTime();
            DateTime endDate = input.EndDate ?? new DateTime();

            var sleepBlocks = SleepBlockRepository.FetchSleepBlocksInDateRange(fc, startDate, endDate);

            if (input.OverwriteBehavior == "DeleteExisting")
            {
                File.Delete(input.RegistryPath);
            }

            var sleepReg = new SleepDataRegistry(input.RegistryPath);

            if (input.OverwriteBehavior == "MergePickExisting")
            {
                var existingSleepBlocks = SleepBlockRepository.FetchSleepBlocksFromFile(sleepReg);
                sleepBlocks = SleepBlockListMerger.SubtractExistingFromList(sleepBlocks, existingSleepBlocks);
            }

            // Print the results (show it's working!)
            foreach (var block in sleepBlocks)
            {
                Console.WriteLine(block.ToString());
            }
            var libSleepBlocks = SleepBlockAdapter.ToSleepBlocks(new List<ISleepBlockAdapter>(sleepBlocks));
            if (sleepReg.WriteSleepData(libSleepBlocks))
            {
                Console.WriteLine("\nSuccessfully wrote new sleep data to {0}!", input.RegistryPath);
            }
            else
            {
                Console.WriteLine("\nFailed to write to file!");
            }
        }
    }
}
