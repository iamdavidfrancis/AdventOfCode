using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day12 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            Dictionary<string, HashSet<string>> caves = new();

            using (TextReader reader = File.OpenText("./2021/Day12Input.txt"))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var segments = line.Split('-');
                    var left = segments[0];
                    var right = segments[1];

                    if (!caves.ContainsKey(left))
                    {
                        caves.Add(left, new HashSet<string>());
                    }

                    if (!caves.ContainsKey(right) && right != "end")
                    {
                        caves.Add(right, new HashSet<string>());
                    }

                    if (right != "start" && left != "end")
                    {
                        caves[left].Add(right);
                    }

                    if (left != "start" && right != "end")
                    {
                        caves[right].Add(left);
                    }
                }
            }

            var total = DFS(caves, "start", new HashSet<string>(), new List<string>());

            Console.WriteLine(total);
        }

        private int DFS(Dictionary<string, HashSet<string>> caves, string start, HashSet<string> visited, List<string> path)
        {
            if (start == "end")
            {
                // Console.WriteLine(path.Aggregate(string.Empty, (acc, c) => acc += $",{c}").Substring(1) + ",end");

                return 1;
            }

            var runningTotal = 0;

            var isSmallCave = start.ToUpper() != start;

            // Don't vist 
            if (isSmallCave)
            {
                if (!visited.Contains(start))
                {
                    visited.Add(start);
                }
            }

            path.Add(start);

            bool isSmallCaveRevisited = path
                .Where(c => c.ToUpper() != c) // Only lowercase
                .GroupBy(c => c) // Group by node
                .Any(g => g.Count() > 1); // Where it's been visited more than once.

            foreach (var adjacentCave in caves[start].OrderBy(c => c))
            {
                if (visited.Contains(adjacentCave) && isSmallCaveRevisited)
                {
                    continue;
                }

                runningTotal += DFS(caves, adjacentCave, visited, path);
            }

            if (isSmallCave)
            {
                var visitedCount = path.Where(c => c == start).Count();

                if (visitedCount == 1)
                {
                    visited.Remove(start);
                }
            }

            path.RemoveAt(path.Count - 1);

            return runningTotal;
        }
    }
}
