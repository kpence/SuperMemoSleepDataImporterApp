using System;
using System.Collections.Generic;
using SleepDataImporter.Models;

namespace FitbitSuperMemoSleepDataImporterApp
{
    class SleepBlockListMerger
    {
        private SleepBlockMergeStrategy MergeStrategy { get; set; }
        public SleepBlockListMerger(SleepBlockMergeStrategy mergeStrategy)
        {
            this.MergeStrategy = mergeStrategy;
        }
        public SleepBlock[] Merge(SleepBlock[] existingSleepBlocks, SleepBlock[] newSleepBlocks)
        {
            var existingSleepBlockIterator = new SleepBlockIterator(existingSleepBlocks);
            var newSleepBlockIterator = new SleepBlockIterator(newSleepBlocks);
            var sleepBlockList = new List<SleepBlock>(MergeStrategy.GetInitialSleepBlocks(existingSleepBlocks, newSleepBlocks));
            var existingSleepBlock = existingSleepBlockIterator.Get();
            var newSleepBlock = newSleepBlockIterator.Get();
            while (existingSleepBlockIterator.hasNext() && newSleepBlockIterator.hasNext())
            {
                if (IsOverlap(existingSleepBlock, newSleepBlock))
                {
                    var pickedSleepBlock = MergeStrategy.PickSleepBlock(existingSleepBlock, newSleepBlock);
                    if (pickedSleepBlock != null)
                    {
                        sleepBlockList.Add(pickedSleepBlock);
                    }
                }
                if (EarlierThan(newSleepBlock, existingSleepBlock))
                {
                    newSleepBlock = newSleepBlockIterator.Next();
                } else
                {
                    existingSleepBlock = existingSleepBlockIterator.Next();
                }
            }
            return sleepBlockList.ToArray();
        } 
        private bool EarlierThan(SleepBlock sleepBlock1, SleepBlock sleepBlock2)
        {
            return sleepBlock1.Start < sleepBlock2.Start;
        }
        private bool IsOverlap(SleepBlock sleepBlock1, SleepBlock sleepBlock2)
        {
            // TODO
            return false;
        }
    }
    public class PickNewStrategy : SleepBlockMergeStrategy
    {
        public SleepBlock? PickSleepBlock(SleepBlock existingSleepBlock, SleepBlock newSleepBlock)
        {
            return newSleepBlock;
        }
        public SleepBlock[] GetInitialSleepBlocks(SleepBlock[] existingSleepBlocks, SleepBlock[] newSleepBlocks)
        {
            return newSleepBlocks;
        }
    }
    public class PickExistingStrategy : SleepBlockMergeStrategy
    {
        public SleepBlock? PickSleepBlock(SleepBlock existingSleepBlock, SleepBlock newSleepBlock)
        {
            return existingSleepBlock;
        }
        public SleepBlock[] GetInitialSleepBlocks(SleepBlock[] existingSleepBlocks, SleepBlock[] newSleepBlocks)
        {
            return existingSleepBlocks;
        }
    }
    interface SleepBlockMergeStrategy
    {
        public SleepBlock? PickSleepBlock(SleepBlock existingSleepBlock, SleepBlock newSleepBlock);
        public SleepBlock[] GetInitialSleepBlocks(SleepBlock[] existingSleepBlocks, SleepBlock[] newSleepBlocks);
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
        public bool hasNext()
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
