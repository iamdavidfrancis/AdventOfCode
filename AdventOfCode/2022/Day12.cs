using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day12 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<List<char>> pointGrid = new();

            using (TextReader reader = File.OpenText("./2022/Day12.txt"))
            {
                string? line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var row = line.ToCharArray().ToList();
                    pointGrid.Add(row);
                }
            }

            var endPoint = pointGrid.FindPoint('E');
            var startPoint = pointGrid.FindPoint('S');

            pointGrid.Set(endPoint, 'z');
            pointGrid.Set(startPoint, 'a');

            var elevationsOfA = pointGrid.FindPoints('a');

            var grid = pointGrid.Map(c => c - 'a');
            Dictionary<Point2D, int> visited = new();

            BFS(grid, endPoint, visited);

            Console.WriteLine($"Part 1: {visited[startPoint]}");

            var min = Int32.MaxValue;
            foreach (var point in elevationsOfA)
            {
                if (visited.TryGetValue(point, out int currMin) && currMin < min)
                {
                    min = visited[point];
                }
            }

            Console.WriteLine($"Part 2: {min}");
        }

        private void BFS(List<List<int>> grid, Point2D endingPoint, Dictionary<Point2D, int> visited)
        {
            var queue = new Queue<Point2D>();
            queue.Enqueue(endingPoint);
            visited.Add(endingPoint, 0);

            while (queue.TryDequeue(out var currPoint))
            {
                var nextPoints = currPoint.Neighbors();
                var validNextPoints = grid.ValidNeighbors(nextPoints);
                foreach (var nextPoint in validNextPoints)
                {
                    if (grid.Get(currPoint) <= grid.Get(nextPoint) + 1 && !visited.ContainsKey(nextPoint))
                    {
                        visited[nextPoint] = visited[currPoint] + 1;
                        queue.Enqueue(nextPoint);
                    }
                }
            }
        }
    }
}

