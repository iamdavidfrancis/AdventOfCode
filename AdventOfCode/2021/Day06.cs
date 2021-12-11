using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2021
{
    internal class Day06 : IAdventOfCodeProblem
    {
        private Dictionary<int, ulong> FishCount;

        public void RunProblem()
        {
            var initialState = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 4, 1, 2, 1, 1, 4, 1, 1, 1, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 1, 1, 1, 3, 1, 1, 2, 1, 2, 1, 3, 3, 4, 1, 4, 1, 1, 3, 1, 1, 5, 1, 1, 1, 1, 4, 1, 1, 5, 1, 1, 1, 4, 1, 5, 1, 1, 1, 3, 1, 1, 5, 3, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 2, 4, 1, 1, 1, 1, 4, 1, 2, 2, 1, 1, 1, 3, 1, 2, 5, 1, 4, 1, 1, 1, 3, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 5, 1, 1, 1, 4, 1, 1, 5, 1, 1, 5, 3, 3, 5, 3, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 5, 3, 1, 2, 1, 1, 1, 4, 1, 3, 1, 5, 1, 1, 2, 1, 1, 1, 1, 1, 5, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 4, 3, 2, 1, 2, 4, 1, 3, 1, 5, 1, 2, 1, 4, 1, 1, 1, 1, 1, 3, 1, 4, 1, 1, 1, 1, 3, 1, 3, 3, 1, 4, 3, 4, 1, 1, 1, 1, 5, 1, 3, 3, 2, 5, 3, 1, 1, 3, 1, 3, 1, 1, 1, 1, 4, 1, 1, 1, 1, 3, 1, 5, 1, 1, 1, 4, 4, 1, 1, 5, 5, 2, 4, 5, 1, 1, 1, 1, 5, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 5, 1, 1, 1, 1, 1, 1, 3, 1, 1, 2, 1, 1 };

            FishCount = InitializeCounts();

            Populate(initialState);

            for (int i = 0; i < 256; i++)
            {
                Iterate();
            }

            var total = GetSum();
            Console.WriteLine(total);
        }

        private void Populate(List<int> initialState)
        {
            foreach (var item in initialState)
            {
                if (FishCount.ContainsKey(item))
                {
                    FishCount[item]++;
                }
                else
                {
                    FishCount[item] = 1;
                }
            }
        }

        private Dictionary<int, ulong> InitializeCounts()
        {
            Dictionary<int, ulong> counts = new();

            counts.Add(0, 0);
            counts.Add(1, 0);
            counts.Add(2, 0);
            counts.Add(3, 0);
            counts.Add(4, 0);
            counts.Add(5, 0);
            counts.Add(6, 0);
            counts.Add(7, 0);
            counts.Add(8, 0);

            return counts;
        }

        private void Iterate()
        {
            Dictionary<int, ulong> newValues = InitializeCounts();

            // Handle the 0 case first
            var fishAtZero = FishCount[0];

            newValues[6] = fishAtZero;
            newValues[8] = fishAtZero;

            for (int i = 1; i <= 8; i++)
            {
                var fish = FishCount[i];
                newValues[i - 1] += fish;
            }

            FishCount = newValues;
        }

        private ulong GetSum()
        {
            ulong sum = 0;

            foreach (var item in FishCount.Values)
            {
                sum += item;
            }

            return sum;
        }
    }
}
