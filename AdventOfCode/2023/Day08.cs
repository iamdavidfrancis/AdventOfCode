using System.Text.RegularExpressions;
using MathNet.Numerics;

namespace AdventOfCode._2023;

public class Day08 : AdventBase
{
    public override string FilePath => "./2023/Day08.txt";

    public override async Task<AdventResult> RunInternal(TextReader reader)
    {
        string? line;
        
        List<string> currentNodesPart2 = [];

        var instructions = await reader.ReadLineAsync();

        instructions ??= "";

        await reader.ReadLineAsync();

        var map = new Dictionary<string, (string Left, string Right)>();

        while ((line = await reader.ReadLineAsync()) != null) 
        {
            var matches = Regex.Match(line, @"(\w{3}) = \((\w{3}), (\w{3})\)");
            var node = matches.Groups[1].Value;
            var left = matches.Groups[2].Value;
            var right = matches.Groups[3].Value;

            map.Add(node, (left, right));

            if (node.EndsWith('A'))
            {
                currentNodesPart2.Add(node);
            }
        }

        // Part 1
        long steps = CalcPath(instructions, map, "AAA", "ZZZ");

        // Part 2
        long steps2 = currentNodesPart2
            .Select(n => CalcPath(instructions, map, n, "Z"))
            .Aggregate(Euclid.LeastCommonMultiple);
        

        return new(steps, steps2);
    }

    private long CalcPath(string instructions, Dictionary<string, (string Left, string Right)> map, string node, string end)
    {
        long count = 0;
        int current = 0;

        while (!node.EndsWith(end))
        {
            node = instructions[current] == 'L' ? map[node].Left : map[node].Right;
            current = (current + 1) % instructions.Length;
            ++count;
        }

        return count;
    }
}