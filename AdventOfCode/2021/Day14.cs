using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day14 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<char> polymer = new();
            Dictionary<string, char> templates = new();

            using (TextReader reader = File.OpenText("./2021/Day14Input.txt"))
            {
                string? line;
                // Get points
                while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync()))
                {
                    polymer.AddRange(line.ToCharArray());
                }

                // Get folds
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var pieces = line.Split(" -> ");
                    var key = pieces[0];
                    var value = pieces[1].First();

                    templates[key] = value;
                }

                var characterCount = polymer.Count;
                var counts = new Dictionary<char, ulong>();
                var pairs = new Dictionary<string, ulong>();

                for (int i = 0; i < characterCount - 1; i++)
                {
                    var pair = new string(polymer.Skip(i).Take(2).ToArray());

                    if (pairs.ContainsKey(pair))
                    {
                        pairs[pair]++;
                    }
                    else
                    {
                        pairs.Add(pair, 1);
                    }

                    if (counts.ContainsKey(polymer[i]))
                    {
                        counts[polymer[i]]++;
                    }
                    else
                    {
                        counts.Add(polymer[i], 1);
                    }
                }

                if (counts.ContainsKey(polymer.Last()))
                {
                    counts[polymer.Last()]++;
                }
                else
                {
                    counts.Add(polymer.Last(), 1);
                }

                for (int i = 0; i < 40; i++)
                {
                    pairs = RunReaction(pairs, templates, counts);
                }

                var orderedCounts = counts.OrderBy(kv => kv.Value);
                var smallest = orderedCounts.First();
                var largest = orderedCounts.Last();

                Console.WriteLine(largest.Value - smallest.Value);
            }
        }

        private Dictionary<string, ulong> RunReaction(Dictionary<string, ulong> chars, Dictionary<string, char> templates, Dictionary<char, ulong> counts)
        {
            Dictionary<string, ulong> newChars = new();

            foreach (var pair in chars.Keys)
            {
                var count = chars[pair];

                var newElement = templates[pair];

                var leftPair = new string(new char[] { pair[0], newElement });
                var rightPair = new string(new char[] { newElement, pair[1] });

                if (newChars.ContainsKey(leftPair))
                {
                    newChars[leftPair] += count;
                }
                else
                {
                    newChars[leftPair] = count;
                }

                if (newChars.ContainsKey(rightPair))
                {
                    newChars[rightPair] += count;
                }
                else
                {
                    newChars[rightPair] = count;
                }

                if (counts.ContainsKey(newElement))
                {
                    counts[newElement] += count;
                }
                else
                {
                    counts[newElement] = count;
                }
            }

            return newChars;
        }
    }
}
