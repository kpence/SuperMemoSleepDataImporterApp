using System;

namespace SuperMemoSleepDataImporterApp
{
    public class InputResponse
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string RegistryPath { get; set; }
        public string OverwriteBehavior { get; set; }
    }
    interface IInputPrompt<T>
    {
        public string UseDefaultConfigKey { get; set; }
        public string DefaultValueConfigKey { get; set; }
        public string Prompt { get; set; }
        public T MapValueFromString(string s);
    }
    public class StringInputPrompt : IInputPrompt<string>
    {
        public string UseDefaultConfigKey { get; set; }
        public string DefaultValueConfigKey { get; set; }
        public string Prompt { get; set; }
        public string MapValueFromString(string s)
        {
            return s;
        }
    }
    public class DateInputPrompt : IInputPrompt<DateTime?>
    {
        public string UseDefaultConfigKey { get; set; }
        public string DefaultValueConfigKey { get; set; }
        public string Prompt { get; set; }
        public DateTime? EarliestAllowedDate { get; set; }
        public DateTime? MapValueFromString(string s)
        {
            var date = InputHelper.GetDateTimeFromString(s);
            return (EarliestAllowedDate != null && date < EarliestAllowedDate)
                ? null
                : date;
        }
    }
    public class FilePathInputPrompt : IInputPrompt<string>
    {
        public string UseDefaultConfigKey { get; set; }
        public string DefaultValueConfigKey { get; set; }
        public string Prompt { get; set; }
        public string MapValueFromString(string s)
        {
            return InputHelper.IsFilePathValid(s) ? s : null;
        }
    }
    public class OverwriteOptionInputPrompt : IInputPrompt<string>
    {
        public string UseDefaultConfigKey { get; set; }
        public string DefaultValueConfigKey { get; set; }
        public string Prompt { get; set; }
        public string MapValueFromString(string s)
        {
            switch (s)
            {
                case "1":
                case "DeleteExisting":
                    return "1";
                case "2":
                case "MergePickExisting":
                    return "2";
                case "3":
                case "Abort":
                    return "3";
            }
            return null;
        }
    }
}
