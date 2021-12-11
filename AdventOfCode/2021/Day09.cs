using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day09 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<List<int>> heightMap = new List<List<int>>();

            using (TextReader reader = File.OpenText("./2021/Day09Input.txt"))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var row = line.Select(c => Convert.ToInt32(new string(new[] {c}))).ToList();
                    heightMap.Add(row);
                }
            }

            var points = FindLowPoints(heightMap);

            var total = points
                .Select(p => GetBasinSize(heightMap, p))
                .OrderByDescending(p => p)
                .Take(3)
                .Aggregate(1, (acc, x) => acc * x);

            Console.WriteLine(total);
        }

        private int GetBasinSize(List<List<int>> heightMap, Point2D lowPoint)
        {
            Queue<Point2D> queue = new();
            HashSet<Point2D> visited = new();

            queue.Enqueue(lowPoint);

            while (queue.Count > 0)
            {
                Point2D currentPoint = queue.Dequeue();

                var neighbors = heightMap.ValidNeighbors(currentPoint.Neighbors())
                    .Where(n => heightMap.Get(n) != 9)
                    .Where(n => !visited.Contains(n));

                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }

            return visited.Count;
        }

        private List<Point2D> FindLowPoints(List<List<int>> heightMap)
        {
            var lowPoints = new List<Point2D>();

            for (int y = 0; y < heightMap.Count; y++)
            {
                var row = heightMap[y];

                for (int x = 0; x < row.Count; x++)
                {
                    var position = new Point2D(x, y);
                    var neighbors = heightMap.ValidNeighbors(position.Neighbors());

                    if (neighbors.All(n => heightMap.Get(position) < heightMap.Get(n)))
                    {
                        lowPoints.Add(position);
                    }
                }
            }

            return lowPoints;
        }
    }

    internal static class HeightMapExtensions
    {
        public static int Get(this List<List<int>> source, Point2D point)
        {
            return source[point.Y][point.X];
        }

        public static void Set(this List<List<int>> source, Point2D point, int value)
        {
            source[point.Y][point.X] = value;
        }

        public static int Increment(this List<List<int>> source, Point2D point)
        {
            return ++source[point.Y][point.X];
        }

        public static bool Contains(this List<List<int>> source, Point2D point)
        {
            return point.Y >= 0 && source.Count > point.Y && point.X >= 0 && source[point.Y].Count > point.X;
        }

        public static IEnumerable<Point2D> ValidNeighbors(this List<List<int>> source, IEnumerable<Point2D> neighbors)
        {
            return neighbors.Where(n => source.Contains(n));
        }
    }
}
