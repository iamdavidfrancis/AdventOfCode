using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2021
{
    internal class Day03 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {

            using (TextReader reader = File.OpenText("./2021/Day03Input.txt"))
            {
                string? line;

                Dictionary<int, int> OneBitCounts = new();
                List<char[]> lines = new List<char[]>();

                int numLines = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(ProcessLine(line, OneBitCounts));
                    numLines++;
                }

                // var (gamma, epsilon, mostCommonBits, leastCommonBits) = GetRates(OneBitCounts, numLines);

                var (oxygen, co2) = GetRatings(lines);

                Console.WriteLine(oxygen * co2);
            }

        }

        private char[] ProcessLine(string line, Dictionary<int, int> bitCounts)
        {
            char[] bits = line.ToCharArray();

            for (int i = 0; i < bits.Length; i++)
            {
                if (bitCounts.ContainsKey(i))
                {
                    if (bits[i] == '1')
                    {
                        bitCounts[i]++;
                    }
                }
                else
                {
                    if (bits[i] == '1')
                    {
                        bitCounts[i] = 1;
                    }
                    else
                    {
                        bitCounts[i] = 0;
                    }
                }
            }

            return bits;
        }

        private (int Gamma, int Epsilon, char[] mostCommonBits, char[] leastCommonBits) GetRates(Dictionary<int, int> bitCounts, int numLines)
        {
            int numBits = bitCounts.Count;
            StringBuilder gammaSb = new StringBuilder();
            StringBuilder epsilonSb = new StringBuilder();
            char[] mostCommonBits = new char[numBits];
            char[] leastCommonBits = new char[numBits];

            for (int i = 0; i < numBits; i++)
            {
                var count = bitCounts[i];
                var doubleCount = count * 2;

                if (doubleCount > numLines)
                {
                    mostCommonBits[i] = '1';
                    leastCommonBits[i] = '0';
                    gammaSb.Append('1');
                    epsilonSb.Append('0');
                }
                else if (doubleCount == numLines)
                {
                    mostCommonBits[i] = '1';
                    leastCommonBits[i] = '0';
                    gammaSb.Append('0');
                    epsilonSb.Append('1');
                }
                else
                {
                    mostCommonBits[i] = '0';
                    leastCommonBits[i] = '1';
                    gammaSb.Append('0');
                    epsilonSb.Append('1');
                }
            }

            int gamma = Convert.ToInt32(gammaSb.ToString(), 2);
            int epsilon = Convert.ToInt32(epsilonSb.ToString(), 2);

            return (gamma, epsilon, mostCommonBits, leastCommonBits);
        }

        private char GetCommonBits(List<char[]> lines, int index, bool getMostCommon)
        {
            int oneBits = 0, zeroBits = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                char bit = lines[i][index];

                if (bit == '1')
                {
                    oneBits++;
                }
                else
                {
                    zeroBits++;
                }
            }

            char mostCommon = oneBits >= zeroBits ? '1' : '0';
            char leastCommon = oneBits >= zeroBits ? '0' : '1';

            return getMostCommon ? mostCommon : leastCommon;
        }

        private (int oxygen, int co2) GetRatings( List<char[]> lines)
        {
            List<char[]> filteredOxygenList = lines.Select(s => s).ToList();
            List<char[]> filteredCo2List = lines.Select(s => s).ToList();

            for (int i = 0; i < lines[0].Length; i++)
            {
                var mostCommon = GetCommonBits(filteredOxygenList, i, true);
                var leastCommon = GetCommonBits(filteredCo2List, i, false);

                bool shouldBreak = true;
                if (filteredOxygenList.Count() > 1)
                {
                    filteredOxygenList = filteredOxygenList.Where(s => s[i] == mostCommon).ToList();
                    shouldBreak = false;
                }

                if (filteredCo2List.Count() > 1)
                {
                    filteredCo2List = filteredCo2List.Where(s => s[i] == leastCommon).ToList();
                    shouldBreak = false;
                }

                if (shouldBreak)
                {
                    break;
                }
            }

            var oxygenStr = new string(filteredOxygenList.Single());
            var co2Str = new string(filteredCo2List.Single());

            Console.WriteLine($"Oxygen: {oxygenStr}");
            Console.WriteLine($"CO2: {co2Str}");

            var oxygen = Convert.ToInt32(oxygenStr, 2);
            var co2 = Convert.ToInt32(co2Str, 2);

            return (oxygen, co2);
        }
    }
}
