using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day15 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<string> lines = new List<string>();

            using (TextReader reader = File.OpenText("./2021/Day15Input.txt"))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            List<List<int>> map = new();

            for (int y = 0; y < lines.Count; y++)
            {
                var row = lines[y].Select(c => Convert.ToInt32(new string(new[] { c }))).ToList();
                map.Add(row);
            }

            var start = new WeightedPoint2D(0, 0, 0);

            var width = map[0].Count;
            var height = map.Count;

            // Part 2

            // To the right
            for (var y = 0; y < height; y++)
            {
                for (var tile = 1; tile < 5; tile++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var newRisk = map.Get(new Point2D(x + (tile - 1) * width, y)) + 1;

                        if (newRisk > 9)
                        {
                            newRisk = 1;
                        }

                        map[y].Add(newRisk);
                    }
                }
            }

            width *= 5;

            // Down
            for (var tile = 1; tile < 5; tile++)
            {
                for (var y = 0; y < height; y++)
                {
                    map.Add(new List<int>());

                    for (var x = 0; x < width; x++)
                    {
                        var newRisk = map.Get(new Point2D(x, y + (tile - 1) * height)) + 1;
                        
                        if (newRisk > 9)
                        {
                            newRisk = 1;
                        }

                        map.Last().Add(newRisk);
                    }
                }
            }

            height *= 5;
            // End Part 2

            var maxX = width - 1;
            var maxY = height - 1;

            var end = new WeightedPoint2D(maxX, maxY, map.Get(new Point2D(maxX, maxY)));

            var total = Search(map, start, end);
            Console.WriteLine(total);
        }

        private static int Search(List<List<int>> graph, WeightedPoint2D start, WeightedPoint2D dest)
        {
            PriorityQueue<WeightedPoint2D, int> priorityQueue = new();
            HashSet<Point2D> visited = new();

            visited.Add(start);
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();

                if (current.X == dest.X && current.Y == dest.Y)
                {
                    return current.Weight;
                }

                var neighbors = graph.ValidNeighbors(current.Neighbors()).Where(n => !visited.Contains(new Point2D(n.X, n.Y)));

                foreach (var neighbor in neighbors)
                {
                    visited.Add(neighbor);
                    priorityQueue.Enqueue(new WeightedPoint2D(neighbor.X, neighbor.Y, current.Weight + graph.Get(neighbor)), current.Weight + graph.Get(neighbor));
                }
            }

            return -1;
        }

        private record WeightedPoint2D(int X, int Y, int Weight) : Point2D(X, Y);
    }
}
