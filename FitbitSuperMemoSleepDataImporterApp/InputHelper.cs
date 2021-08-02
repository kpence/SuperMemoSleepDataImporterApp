using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FitbitSuperMemoSleepDataImporterApp
{
    class InputHelper
    {
        public static InputResponse GetInputResponseFromUserPrompts()
        {
            var startDatePrompt = new DateInputPrompt();
            var endDatePrompt = new DateInputPrompt();
            var registryPathPrompt = new FilePathInputPrompt();
            var overwriteBehaviorPrompt = new OverwriteOptionInputPrompt();

            startDatePrompt.DefaultValueConfigKey = "DefaultStartDate";
            endDatePrompt.DefaultValueConfigKey = "DefaultEndDate";
            registryPathPrompt.DefaultValueConfigKey = "DefaultSleepDataRegistryPath";
            overwriteBehaviorPrompt.DefaultValueConfigKey = "DefaultOverwriteBehavior";

            startDatePrompt.UseDefaultConfigKey = "UseDefaultStartDate";
            endDatePrompt.UseDefaultConfigKey = "UseDefaultEndDate";
            registryPathPrompt.UseDefaultConfigKey = "UseDefaultSleepDataRegistryPath";
            overwriteBehaviorPrompt.UseDefaultConfigKey = "UseDefaultOverwriteBehavior";

            var dateExplanation = "\tDates can be in the following formats:\n" +
                "\t\tYYYY-MM-DD or 0 (meaning today) or -X (which means X days before today)";

            startDatePrompt.Prompt = "Enter in the start date.\n" + dateExplanation;
            endDatePrompt.Prompt = "Enter in the end date.\n" + dateExplanation;
            registryPathPrompt.Prompt = "Enter in file path of Sleep Registry file.";
            overwriteBehaviorPrompt.Prompt = "File already exists. Choose an option:\n" +
                "\t1 (DeleteExisting) - Delete existing file under path, and create new one.\n" +
                "\t2 (MergePickExisting) - Merge new data with data in existing file under path, and if any sleep blocks overlap, delete the new sleep block.\n" +
                "\t3 (MergePickNew) - Merge new data with data in existing file under path, and if any sleep blocks overlap, delete the existing sleep block.\n" +
                "\t4 (Abort) - Abort. Do not overwrite.\n" +
                "Enter in number (or label) of option.";

            var response = new InputResponse();
            response.StartDate = GetInput<DateTime?>(startDatePrompt);
            endDatePrompt.EarliestAllowedDate = response.StartDate;
            response.EndDate = GetInput<DateTime?>(endDatePrompt);
            response.RegistryPath = GetInput<string>(registryPathPrompt);
            response.OverwriteBehavior = (File.Exists(response.RegistryPath))
                ?  GetInput<string>(overwriteBehaviorPrompt)
                : null;
            return response;
        }

        public static T GetInput<T>(IInputPrompt<T> inputPrompt)
        {
            T inputValue = default;
            string userInputString = "";
            while (EqualityComparer<T>.Default.Equals(inputValue, default))
            {
                var defaultValue = Options.Get(inputPrompt.DefaultValueConfigKey);
                var useDefaultValue = Options.Get(inputPrompt.UseDefaultConfigKey);
                if (useDefaultValue == "True")
                {
                    return inputPrompt.MapValueFromString(defaultValue);
                }
                Console.WriteLine(inputPrompt.Prompt);

                if (String.IsNullOrEmpty(defaultValue))
                {
                    Console.Write(String.Format(" : "));
                }
                    else
                {
                    Console.Write(String.Format(" [Leave Blank for Default: {0}]: ", defaultValue));
                }

                userInputString = Console.ReadLine();
                Console.WriteLine("");
                var inputString = String.IsNullOrEmpty(userInputString)
                    ? defaultValue
                    : userInputString;

                inputValue = inputPrompt.MapValueFromString(inputString);
            }
            while (true)
            {
                if (userInputString == "" || userInputString == inputPrompt.DefaultValueConfigKey)
                {
                    break;
                }
                var s = String.Format("Would you like to save this as default? y/[N]: ");
                Console.Write(s);
                var confirmUserInputString = Console.ReadLine().ToLower();
                Console.WriteLine("");
                if (String.IsNullOrEmpty(confirmUserInputString) || confirmUserInputString == "n")
                {
                    break;
                }
                else if (confirmUserInputString == "y")
                {
                    Options.Set(inputPrompt.DefaultValueConfigKey, userInputString);
                    break;
                }
            }

            return inputValue;
        }

        public static DateTime? GetDateTimeFromString(string dateString)
        {
            string pattern = @"(^\d{4}-([0]\d|1[0-2])-([0-2]\d|3[01])$)|(^-\d*$)|(^0$)";
            Regex r = new Regex(pattern);
            Match m = r.Match(dateString);

            if (!m.Success)
            {
                return null;
            }

            Group absoluteDateGroup = m.Groups[1];
            Group relativeDateGroup = m.Groups[4];
            Group todayDateGroup = m.Groups[5];

            if (absoluteDateGroup.Success)
            {
                return DateTime.Parse(absoluteDateGroup.Value);
            }
            if (relativeDateGroup.Success)
            {
                int days = Int32.Parse(relativeDateGroup.Value);
                return DateTime.Today + TimeSpan.FromDays(days);
            }
            if (todayDateGroup.Success)
            {
                return DateTime.Today;
            }

            return null;
        }

        public static bool IsFilePathValid(string filePath)
        {
            var path = Path.GetDirectoryName(filePath);
            return true;
        }
    }
}
