using Xunit;
using System;
using System.Collections.Generic;

namespace SuperMemoSleepDataImporterApp.Tests
{
    public class SleepBlockListMergerTests
    {
        private readonly List<ISleepBlockAdapter> ExistingSleepBlocks = new List<ISleepBlockAdapter>
        {
            new SleepBlockAdapter
            {
                Start = new DateTime(2021, 1, 1, 4, 0, 0),
                End = new DateTime(2021, 1, 1, 8, 0, 0)
            },
            new SleepBlockAdapter
            {
                Start = new DateTime(2021, 1, 2, 4, 0, 0),
                End = new DateTime(2021, 1, 2, 8, 0, 0)
            }
        };

        private readonly Dictionary<string, Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>> SleepBlockTestList =
            new Dictionary<string, Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>>
        {
            {
                "Empty New List",
                new Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>
                (
                    new List<ISleepBlockAdapter> { },
                    new List<ISleepBlockAdapter> { }
                )
            },
            {
                "Sorted, No Overlap, All After",
                new Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>
                (
                    new List<ISleepBlockAdapter>
                    {
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 3, 1, 0, 0),
                            End = new DateTime(2021, 1, 3, 8, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 4, 1, 0, 0),
                            End = new DateTime(2021, 1, 4, 8, 0, 0)
                        }
                    },
                    new List<ISleepBlockAdapter>
                    {
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 3, 1, 0, 0),
                            End = new DateTime(2021, 1, 3, 8, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 4, 1, 0, 0),
                            End = new DateTime(2021, 1, 4, 8, 0, 0)
                        }
                    }
                )
            },
            {
                "Sorted, No Overlap, All Before",
                new Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>
                (
                    new List<ISleepBlockAdapter>
                    {
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 1, 1, 0, 0),
                            End = new DateTime(2021, 1, 1, 2, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 4, 2, 0, 0),
                            End = new DateTime(2021, 1, 4, 3, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 4, 3, 0, 0),
                            End = new DateTime(2021, 1, 4, 4, 0, 0)
                        }
                    },
                    new List<ISleepBlockAdapter>
                    {
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 1, 1, 0, 0),
                            End = new DateTime(2021, 1, 1, 2, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 4, 2, 0, 0),
                            End = new DateTime(2021, 1, 4, 3, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 4, 3, 0, 0),
                            End = new DateTime(2021, 1, 4, 4, 0, 0)
                        }
                    }
                )
            },
            {
                "Unsorted - No Convergence",
                new Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>
                (
                    new List<ISleepBlockAdapter>
                    {
                    },
                    new List<ISleepBlockAdapter>
                    {
                    }
                )
            },
            {
                "Overlap1",
                new Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>
                (
                    new List<ISleepBlockAdapter>
                    {
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 1, 3, 0, 0),
                            End = new DateTime(2021, 1, 1, 9, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 2, 1, 0, 0),
                            End = new DateTime(2021, 1, 2, 7, 0, 0)
                        }
                    },
                    new List<ISleepBlockAdapter>
                    {
                    }
                )
            },
            {
                "Partial Overlap",
                new Tuple<List<ISleepBlockAdapter>, List<ISleepBlockAdapter>>
                (
                    new List<ISleepBlockAdapter>
                    {
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 1, 1, 4, 0, 0),
                            End = new DateTime(2021, 1, 1, 8, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2020, 1, 2, 4, 0, 0),
                            End = new DateTime(2021, 1, 3, 8, 0, 0)
                        },
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 3, 2, 4, 0, 0),
                            End = new DateTime(2021, 3, 2, 8, 0, 0)
                        }
                    },
                    new List<ISleepBlockAdapter>
                    {
                        new SleepBlockAdapter
                        {
                            Start = new DateTime(2021, 3, 2, 4, 0, 0),
                            End = new DateTime(2021, 3, 2, 8, 0, 0)
                        }
                    }
                )
            }
        };

        public static IEnumerable<object[]> TestKeys => new List<object[]>
        {
            new object[] { "Empty New List" },
            new object[] { "Sorted, No Overlap, All After" },
            new object[] { "Sorted, No Overlap, All Before" },
            new object[] { "Unsorted - No Convergence" },
            new object[] { "Partial Overlap" },
            new object[] { "Overlap1" },
        };

        private void CompareSleepBlockLists(List<ISleepBlockAdapter> sleepBlockList1, List<ISleepBlockAdapter> sleepBlockList2)
        {
            Assert.Equal(sleepBlockList1.Count, sleepBlockList2.Count);

            for (int i = 0; i < sleepBlockList1.Count; i++)
            {
                Assert.Equal(sleepBlockList1[i].Start, sleepBlockList2[i].Start);
                Assert.Equal(sleepBlockList1[i].End, sleepBlockList2[i].End);
            }
        }

        private void TestSleepBlockList(string TestKey)
        {
            var Results = new List<ISleepBlockAdapter>(
                SleepBlockListMerger.SubtractExistingFromList(
                    SleepBlockTestList[TestKey].Item1.ToArray(),
                    ExistingSleepBlocks.ToArray()
                )
            );
            CompareSleepBlockLists(Results, SleepBlockTestList[TestKey].Item2);
        }

        [Fact]
        public void TestOverlapMethod()
        {
            var sb = new SleepBlockAdapter
            {
                Start = new DateTime(2021, 7, 1, 2, 0, 0),
                End = new DateTime(2021, 7, 1, 8, 0, 0)
            };
            var sb_overlap1 = new SleepBlockAdapter
            {
                Start = new DateTime(2021, 7, 1, 1, 0, 0),
                End = new DateTime(2021, 7, 1, 3, 0, 0)
            };
            var sb_overlap2 = new SleepBlockAdapter
            {
                Start = new DateTime(2021, 7, 1, 6, 0, 0),
                End = new DateTime(2021, 7, 1, 9, 0, 0)
            };
            var sb_overlap3 = new SleepBlockAdapter
            {
                Start = new DateTime(2021, 7, 1, 3, 0, 0),
                End = new DateTime(2021, 7, 1, 6, 0, 0)
            };
            var sb_overlap4 = new SleepBlockAdapter
            {
                Start = new DateTime(2021, 7, 1, 1, 0, 0),
                End = new DateTime(2021, 7, 1, 9, 0, 0)
            };
            var sb_overlap5 = new SleepBlockAdapter
            {
                Start = new DateTime(1999, 7, 1, 1, 0, 0),
                End = new DateTime(2221, 7, 1, 9, 0, 0)
            };
            var sb_noOverlap1 = new SleepBlockAdapter
            {
                Start = new DateTime(2021, 7, 1, 9, 0, 0),
                End = new DateTime(2021, 7, 1, 10, 0, 0)
            };
            var sb_noOverlap2 = new SleepBlockAdapter
            {
                Start = new DateTime(2021, 7, 1, 1, 0, 0),
                End = new DateTime(2021, 7, 1, 1, 30, 0)
            };

            Assert.True(SleepBlockAdapter.IsOverlap(sb, sb_overlap1));
            Assert.True(SleepBlockAdapter.IsOverlap(sb, sb_overlap2));
            Assert.True(SleepBlockAdapter.IsOverlap(sb, sb_overlap3));
            Assert.True(SleepBlockAdapter.IsOverlap(sb, sb_overlap4));
            Assert.True(SleepBlockAdapter.IsOverlap(sb, sb_overlap5));
            Assert.True(sb.IsOverlap(sb_overlap1));
            Assert.True(sb.IsOverlap(sb_overlap2));
            Assert.True(sb.IsOverlap(sb_overlap3));
            Assert.True(sb.IsOverlap(sb_overlap4));
            Assert.True(sb.IsOverlap(sb_overlap5));
            Assert.False(SleepBlockAdapter.IsOverlap(sb, sb_noOverlap1));
            Assert.False(SleepBlockAdapter.IsOverlap(sb, sb_noOverlap2));
        }

        [Theory]
        [MemberData(nameof(TestKeys))]
        public void TestSortedNoOverlap(string key)
        {
            TestSleepBlockList(key);
        }
    }
}
