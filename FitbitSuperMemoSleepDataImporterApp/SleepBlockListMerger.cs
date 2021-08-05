using System;
using System.Collections.Generic;

namespace FitbitSuperMemoSleepDataImporterApp
{
    public class SleepBlockComp : IComparer<ISleepBlockAdapter>
    {
        public int Compare(ISleepBlockAdapter x, ISleepBlockAdapter y)
        {
            return x.Start.CompareTo(y.Start);
        }
    }
    class SleepBlockListMerger
    {
        public static ISleepBlockAdapter[] SubtractExistingFromList(
            ISleepBlockAdapter[] newSleepBlocks,
            ISleepBlockAdapter[] existingSleepBlocks
        ) {
            var comp = new SleepBlockComp();
            Array.Sort(existingSleepBlocks, comp);
            Array.Sort(newSleepBlocks, comp);

            var existingSleepBlockIterator = new SleepBlockIterator(existingSleepBlocks);
            var newSleepBlockIterator = new SleepBlockIterator(newSleepBlocks);

            var sleepBlockList = new List<ISleepBlockAdapter>();

            while (existingSleepBlockIterator.HasNext() && newSleepBlockIterator.HasNext())
            {
                if (!IsOverlap(newSleepBlockIterator.Get(), existingSleepBlockIterator.Get()))
                {
                    if (newSleepBlockIterator.Get().Start > existingSleepBlockIterator.Get().End)
                    {
                        existingSleepBlockIterator.Next();
                    }
                    else
                    {
                        sleepBlockList.Add(newSleepBlockIterator.Get());
                        newSleepBlockIterator.Next();
                    }
                }
                else
                {
                    newSleepBlockIterator.Next();
                }
            }
            while (newSleepBlockIterator.HasNext())
            {
                sleepBlockList.Add(newSleepBlockIterator.Next());
            }
            return sleepBlockList.ToArray();
        }
        private static bool IsDateTimeWithinSleepBlock(DateTime dt, ISleepBlockAdapter sleepBlock)
        {
            return dt >= sleepBlock.Start && dt <= sleepBlock.End;
        }
        private static bool IsOverlap(ISleepBlockAdapter sleepBlock1, ISleepBlockAdapter sleepBlock2)
        {
            return IsDateTimeWithinSleepBlock(sleepBlock1.Start, sleepBlock2)
                || IsDateTimeWithinSleepBlock(sleepBlock2.Start, sleepBlock1);
        }
    }
    public class SleepBlockIterator
    {
        public int CurrentBlockIndex { get; set; }
        private ISleepBlockAdapter[] SleepBlocks { get; set; }
        public SleepBlockIterator(ISleepBlockAdapter[] sleepBlocks)
        {
            SleepBlocks = sleepBlocks;
            CurrentBlockIndex = 0;
        }
        public bool HasNext()
        {
            return (CurrentBlockIndex < SleepBlocks.Length);
        }
        public ISleepBlockAdapter Get()
        {
            return SleepBlocks[CurrentBlockIndex];
        }
        public ISleepBlockAdapter Next()
        {
            var ret = Get();
            this.CurrentBlockIndex += 1;
            return ret;
        }
        public bool EarlierThan(SleepBlockIterator other)
        {
            return Get().Start < other.Get().Start;
        }
    }
}
