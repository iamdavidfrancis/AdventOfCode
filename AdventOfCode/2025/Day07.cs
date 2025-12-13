using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AoC2025
{
    public class Day07 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            Matrix<char> grid;

            using (TextReader reader = File.OpenText("./2025/Day07.txt"))
            {
                grid = await reader.ReadFileToMatrix<char>();
            }

            var start = grid.FindPoint('S');

            var beams = new Dictionary<int, long>();
            beams[start.X] = 1;

            int part1 = 0;

            foreach (var row in grid)
            {
                var newBeams = new Dictionary<int, long>();

                foreach (var (key, value) in beams)
                {
                    if (row[key] == '.')
                    {
                        newBeams.AddOrSet(key, value);
                    }
                    else
                    {
                        part1++;
                        newBeams.AddOrSet(key - 1, value);
                        newBeams.AddOrSet(key + 1, value);
                    }
                }
                beams = newBeams;
            }

            var part2 = beams.Values.Sum();

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }

        // Inital attempt at P1
        private async Task Part1(Matrix<char> grid, Point2D start)
        {
            Queue<Point2D> queue = [];
            queue.Enqueue(start);

            var splitCount = 0;
            while (queue.Count > 0)
            {
                var point = queue.Dequeue();

                if (!grid.Contains(point.Up()))
                {
                    continue;
                }

                var nextPoint = point.Up();

                if (grid.Get(nextPoint) == '.')
                {
                    grid.Set(nextPoint, '|');
                    queue.Enqueue(nextPoint);
                }
                else if (grid.Get(nextPoint) == '^')
                {
                    var splitPoints = grid.ValidNeighbors([nextPoint.Left(), nextPoint.Right()]);

                    var shouldIncrement = true;
                    foreach (var splitPoint in splitPoints)
                    {
                        if (grid.Get(splitPoint) != '.')
                        {
                            shouldIncrement = false;
                        }

                        grid.Set(splitPoint, '|');
                        queue.Enqueue(splitPoint);
                    }

                    if (shouldIncrement || true)
                    {
                        splitCount += 1;
                    }
                }
            }

            Console.WriteLine(splitCount);
            // grid.IterateOverEntireMatrix(p => Console.Write(grid.Get(p)), () => Console.WriteLine());
        }

        private Matrix<char> CloneMatrix(Matrix<char> matrix)
        {
            var result = new Matrix<char>();

            foreach (var row in matrix)
            {
                result.Add([..row]);
            }

            return result;
        }
    }
}
