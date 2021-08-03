using System;
using System.Collections.Generic;
using SleepDataImporter.Models;

namespace FitbitSuperMemoSleepDataImporterApp
{
    public class SleepBlockComp : IComparer<SleepBlock>
    {
        // Compares by Height, Length, and Width.
        public int Compare(SleepBlock x, SleepBlock y)
        {
            return x.Start.CompareTo(y.Start);
        }
    }
    class SleepBlockListMerger
    {
        public static SleepBlock[] SubtractExistingFromList(SleepBlock[] newSleepBlocks, SleepBlock[] existingSleepBlocks)
        {
            var comp = new SleepBlockComp();
            Array.Sort(existingSleepBlocks, comp);
            Array.Sort(newSleepBlocks, comp);

            var existingSleepBlockIterator = new SleepBlockIterator(existingSleepBlocks);
            var newSleepBlockIterator = new SleepBlockIterator(newSleepBlocks);

            var sleepBlockList = new List<SleepBlock>();

            while (existingSleepBlockIterator.HasNext() && newSleepBlockIterator.HasNext())
            {
                if (EarlierThan(newSleepBlockIterator.Get(), existingSleepBlockIterator.Get()))
                {
                    if (!IsOverlap(existingSleepBlockIterator.Get(), newSleepBlockIterator.Get()))
                    {
                        sleepBlockList.Add(newSleepBlockIterator.Get());
                    }
                    newSleepBlockIterator.Next();
                } else
                {
                    existingSleepBlockIterator.Next();
                }
            }
            while (newSleepBlockIterator.HasNext())
            {
                sleepBlockList.Add(newSleepBlockIterator.Next());
            }
            return sleepBlockList.ToArray();
        } 
        private static bool EarlierThan(SleepBlock sleepBlock1, SleepBlock sleepBlock2)
        {
            return sleepBlock1.Start < sleepBlock2.Start;
        }
        private static bool IsOverlap(SleepBlock sleepBlock1, SleepBlock sleepBlock2)
        {
            return !(sleepBlock1.Start > sleepBlock2.End || sleepBlock2.Start > sleepBlock1.End);
        }
    }
    public class SleepBlockIterator
    {
        private int CurrentBlockIndex { get; set; }
        private SleepBlock[] SleepBlocks { get; set; }
        public SleepBlockIterator(SleepBlock[] sleepBlocks)
        {
            this.SleepBlocks = sleepBlocks;
            this.CurrentBlockIndex = 0;
        }
        public bool HasNext()
        {
            return (CurrentBlockIndex < SleepBlocks.Length);
        }
        public SleepBlock Get()
        {
            return SleepBlocks[CurrentBlockIndex];
        }
        public SleepBlock Next()
        {
            var ret = Get();
            this.CurrentBlockIndex += 1;
            return ret;
        }
    }
}
