using MathNet.Numerics.Providers.LinearAlgebra;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023;

public class Day02 : IAsyncAdventOfCodeProblem
{
    private Dictionary<string, int> MaxCubes = new()
    {
        { "red", 12 },
        { "blue", 14 },
        { "green", 13 },
    };

    public async Task RunProblemAsync()
    {
        using (TextReader reader = File.OpenText("./2023/Day02.txt"))
        {
            string gameIdPattern = "Game (\\d+):";
            string drawsPattern = "([a-zA-Z0-9 ,]+?)(?:;|$)";
            string valuePattern = "(\\d+) ([a-zA-Z]+)";
            
            string? line;

            var sum1 = 0;
            var sum2 = 0;

            while ((line = await reader.ReadLineAsync()) != null) 
            {
                var gameId = Regex.Match(line, gameIdPattern).Groups[1].Value;
                var draws = Regex.Matches(line, drawsPattern);

                var isValid = true;

                Dictionary<string, int> fewestCubes = new()
                {
                    { "red", 0 },
                    { "blue", 0 },
                    { "green", 0 },
                };

                foreach (Match draw in draws)
                {
                    var parsedDraw = draw.Groups[1].Value;
                    var individualDraws = Regex.Matches(parsedDraw, valuePattern);

                    foreach (Match individualDraw in  individualDraws)
                    {
                        var count = int.Parse(individualDraw.Groups[1].Value);
                        var color = individualDraw.Groups[2].Value;

                        if (count > MaxCubes[color])
                        {
                            isValid = false;
                        }

                        if (count > fewestCubes[color])
                        {
                            fewestCubes[color] = count;
                        }
                    }
                }

                sum2 += fewestCubes.Values.Aggregate(1, (acc, minCubes) => acc * minCubes);

                if (isValid)
                {
                    sum1 += int.Parse(gameId);
                }
            }

            Console.WriteLine("Part 1");
            Console.WriteLine(sum1);

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(sum2);
        }
    }
}