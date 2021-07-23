using System;
using System.Collections.Generic;
using Fitbit.Api.Portable;
using Fitbit.Models;
using SleepDataImporter;

namespace FitbitSuperMemoSleepDataImporterApp
{
	public class Program
    {
        static void Main(string[] args)
        {
            //var file = @"C:\Users\james\SuperMemo\sleep\sleep.tim";
            //var sleepReg = new SleepDataRegistry(file);
            //sleepReg.ReadSleepData();
            /*
            var credentials = new FitbitAppCredentials();
            credentials.ClientId = "";
            credentials.ClientSecret = "";
            FitbitClient client = new FitbitClient(credentials, null);
            */
            // Signature of method which may be useful: public Task<SleepLogListBase> GetSleepLogListAsync(DateTime dateToList, SleepEnum decisionDate, SortEnum sort, int limit, string encodedUserId = null);

            // Authorize with FitBit
            FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("activity ", "nutrition ", "heartrate ", "location ", "nutrition ", "profile ", "settings ", "sleep ", "social ", "weight");

            // Retrieve the weight information for today
            DateTime start = DateTime.Now;
            Console.Write("Processing {0}...", start.ToShortDateString());
            var weight = fc.GetWeightAsync(start, DateRangePeriod.OneMonth).Result;
            Console.WriteLine("found {0} entries.", weight.Weights.Count);

            // Save the downloaded information to disk
            System.Xml.Serialization.XmlSerializer src = new System.Xml.Serialization.XmlSerializer(typeof(List<Weight>));

        }
    }
}
