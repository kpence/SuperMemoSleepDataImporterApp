using System;
using System.Linq;
using System.Collections.Generic;
using SleepDataImporter.Models;

namespace FitbitSuperMemoSleepDataImporterApp
{
    public interface ISleepBlockAdapter
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool NaturalWake { get; set; }
        public bool NaturalToBed { get; set; }
        public SleepBlock ToSleepBlock();
    }
    public class SleepBlockAdapter : ISleepBlockAdapter
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool NaturalWake { get; set; }
        public bool NaturalToBed { get; set; }
        public static ISleepBlockAdapter FromSleepBlock(SleepBlock sleepBlock)
        {
            return new SleepBlockAdapter
            {
                Start = sleepBlock.Start,
                End = sleepBlock.End
            };
        }
        public SleepBlock ToSleepBlock()
        {
            return new SleepBlock(Start, End);
        }
        public static List<SleepBlock> ToSleepBlocks(List<ISleepBlockAdapter> sleepBlocks)
        {
            List<SleepBlock> libSleepBlocks = sleepBlocks.Select(block => block.ToSleepBlock()).ToList();
            return libSleepBlocks;
        }
        public override string ToString()
        {
            return $"Start = {Start}; End = {End};";
        }
    }
}
