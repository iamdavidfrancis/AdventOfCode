using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day11 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<List<int>> map = new List<List<int>>();

            using (TextReader reader = File.OpenText("./2021/Day11Input.txt"))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var row = line.Select(c => Convert.ToInt32(new string(new[] {c}))).ToList();
                    map.Add(row);
                }
            }

            //int total = 0;

            for (int i = 0; i < 1000; i++)
            {
                var flashPoints = Iterate(map);
                
                foreach (var flashPoint in flashPoints)
                {
                    FloodFillLights(map, flashPoint);
                }

                if (SettleFlashes(map) == 100)
                {
                    // Part 2
                    PrintMap(map, i+1);
                    return;
                }

                //total += SettleFlashes(map);
            }

            // Part 1
            //Console.WriteLine(total);
        }

        private int SettleFlashes(List<List<int>> map)
        {
            int flashes = 0;

            for (int y = 0; y < map.Count; y++)
            {
                var row = map[y];

                for (int x = 0; x < row.Count; x++)
                {
                    var position = new Point2D(x, y);

                    if (map.Get(position) > 9)
                    {
                        map.Set(position, 0);
                        flashes++;
                    }
                }
            }

            return flashes;
        }

        private List<Point2D> Iterate(List<List<int>> map)
        {
            var overNine = new List<Point2D>();

            for (int y = 0; y < map.Count; y++)
            {
                var row = map[y];

                for (int x = 0; x < row.Count; x++)
                {
                    var position = new Point2D(x, y);

                    if(map.Increment(position) > 9)
                    {
                        overNine.Add(position);
                    }
                }
            }

            return overNine;
        }

        private void FloodFillLights(List<List<int>> map, Point2D startingPoint)
        {
            Queue<Point2D> queue = new();
            HashSet<Point2D> visited = new();

            queue.Enqueue(startingPoint);
            visited.Add(startingPoint);

            while (queue.Count > 0)
            {
                Point2D currentPoint = queue.Dequeue();

                var neighbors = map.ValidNeighbors(currentPoint.NeighborsWithDiagonal())
                    .Where(n => map.Increment(n) == 10)
                    .Where(n => !visited.Contains(n));

                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        private void PrintMap(List<List<int>> map, int iteration)
        {
            Console.WriteLine($"Iteration {iteration}");
            for (int y = 0; y < map.Count; y++)
            {
                var row = map[y];

                for (int x = 0; x < row.Count; x++)
                {
                    var position = new Point2D(x, y);

                    Console.Write(map.Get(position));
                }
                Console.WriteLine(string.Empty);
            }

            Console.WriteLine(string.Empty);
        }
    }
}
