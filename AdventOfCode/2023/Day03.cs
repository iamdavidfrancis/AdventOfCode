using System.Drawing;
using MathNet.Numerics.Random;

namespace AdventOfCode._2023;

public class Day03 : IAsyncAdventOfCodeProblem
{

    public async Task RunProblemAsync()
    {
        using (TextReader reader = File.OpenText($"./2023/Day03.txt"))
        {
            var matrix = await reader.ReadFileToMatrix(rightPad: '.');

            int sum1 = 0;
            Dictionary<Point2D, List<int>> gears = [];
            
            var width = matrix.Width;
            var height = matrix.Height;

            for (int y = 0; y < height; y++)
            {
                int number = 0;
                bool isValid = false;
                List<Point2D> stars = [];

                for (int x = 0; x < width; x++)
                {
                    var point = new Point2D(x, y);
                    var item = matrix.Get(point);
                    if (!char.IsDigit(item) || x == width + 1)
                    {
                        if (number == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (isValid)
                            {
                                sum1 += number;

                                foreach (var starPoint in stars)
                                {
                                    if (gears.TryGetValue(starPoint, out List<int>? value))
                                    {
                                        value.Add(number);
                                    }
                                    else
                                    {
                                        gears[starPoint] = [number];
                                    }
                                }
                            }

                            number = 0;
                            isValid = false;
                            stars = [];
                        }
                    }
                    else
                    {
                        number = (number * 10) + (item - '0');
                        foreach (var neighbor in matrix.ValidNeighbors(point.NeighborsWithDiagonal()))
                        {
                            var neighborItem = matrix.Get(neighbor);
                            if (!char.IsDigit(neighborItem) && neighborItem != '.')
                            {
                                isValid = true;
                            }

                            if (neighborItem == '*' && !stars.Contains(neighbor))
                            {
                                stars.Add(neighbor);
                            }
                        }
                    }
                }
            }

            var sum2 = gears.Where(g => g.Value.Count == 2).Select(g => g.Value[0] * g.Value[1]).Sum();

            Console.WriteLine("Part 1");
            Console.WriteLine(sum1);

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(sum2);
        }
    }

    private void CheckPositionPart1(Matrix<string> matrix, Point2D point, int sum)
    {
        var item = matrix.Get(point);

        if (int.TryParse(item, out int itemVal))
        {
            foreach (var neighbor in matrix.ValidNeighbors(point.NeighborsWithDiagonal()))
            {
                var neighborItem = matrix.Get(neighbor);
                if (neighborItem != "." && !int.TryParse(neighborItem, out _))
                {
                    sum += itemVal;
                    break;
                }
            }
        }
    }
}