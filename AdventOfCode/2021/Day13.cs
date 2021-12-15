using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day13 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<Point2D> map = new();
            List<Fold> folds = new();

            using (TextReader reader = File.OpenText("./2021/Day13Input.txt"))
            {
                string? line;
                // Get points
                while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync()))
                {
                    var points = line.Split(',');
                    map.Add(new Point2D(Convert.ToInt32(points[0]), Convert.ToInt32(points[1])));
                }

                // Get folds
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var regex = new Regex("fold along (.)=(\\d+)", RegexOptions.Compiled);
                    var groups = regex.Matches(line);

                    var axis = groups[0].Groups[1].Value == "y" ? Axis.Y : Axis.X;

                    folds.Add(new Fold(axis, Convert.ToInt32(groups[0].Groups[2].Value)));
                }

                foreach (var fold in folds)
                {
                    FoldMap(map, fold);
                }

                PrintMap(map);
            }
        }

        private void FoldMap(List<Point2D> map, Fold fold)
        {
            var lineNumber = fold.Line;

            for (int i = 0; i < map.Count; i++)
            {
                var shouldFold = fold.Axis == Axis.X
                    ? map[i].X > lineNumber
                    : map[i].Y > lineNumber;

                if (!shouldFold)
                {
                    continue;
                }

                if (fold.Axis == Axis.X)
                {
                    var dist = map[i].X - lineNumber;
                    var position = lineNumber - dist;

                    map[i] = map[i] with { X = position };
                }
                else
                {
                    var dist = map[i].Y - lineNumber;
                    var position = lineNumber - dist;

                    map[i] = map[i] with { Y = position };
                }
            }
        }

        private void PrintMap(List<Point2D> map)
        {
            var maxX = map.MaxBy(x => x.X)!.X + 1;
            var maxY = map.MaxBy(x => x.Y)!.Y + 1 ;

            char?[,] lines = new char?[maxX, maxY];

            var count = 0;

            foreach (var item in map)
            {
                lines[item.X, item.Y] = '#';
            }

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (lines[x, y] != null)
                    {
                        count++;
                    }

                    Console.Write(lines[x, y] ?? '.');
                }

                Console.WriteLine(string.Empty);
            }

            // Part 1
            // Console.WriteLine(count);
        }

        private enum Axis
        {
            X,
            Y
        }

        private record Fold(Axis Axis, int Line);
    }
}
