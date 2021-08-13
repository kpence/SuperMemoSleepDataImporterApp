using System;
using System.Linq;
using System.Collections.Generic;
using SleepDataImporter.Models;

namespace SuperMemoSleepDataImporterApp
{
    public interface ISleepBlockAdapter
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool NaturalWake { get; set; }
        public bool NaturalToBed { get; set; }
        public SleepBlock ToSleepBlock();
        public bool IsOverlap(ISleepBlockAdapter other);
        public bool IsDateTimeWithinSleepBlock(DateTime dt);
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
        public static bool IsOverlap(ISleepBlockAdapter sleepBlock1, ISleepBlockAdapter sleepBlock2)
        {
            return sleepBlock1.IsOverlap(sleepBlock2);
        }
        public bool IsOverlap(ISleepBlockAdapter other)
        {
            return IsDateTimeWithinSleepBlock(other.Start)
                || other.IsDateTimeWithinSleepBlock(Start);
        }
        public bool IsDateTimeWithinSleepBlock(DateTime dt)
        {
            return dt >= Start && dt <= End;
        }
    }
}
