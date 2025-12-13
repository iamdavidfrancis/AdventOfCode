using AdventOfCode.Helpers.IntervalTree;
using System.Linq;

namespace AdventOfCode.AoC2025;

public class Day06 : IAsyncAdventOfCodeProblem
{
    public async Task RunProblemAsync()
    {
        List<string> lines = [];

        using (TextReader reader = File.OpenText("./2025/Day06.txt"))
        {
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.Add(line);
            }
        }

        var operators = lines.Last();
        var height = lines.Count - 1;

        string currentNumber = string.Empty;
        List<ulong> currentNumbers = [];
        List<ulong> results = [];

        for (int i = operators.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < height; j++)
            {
                var currentChar = lines[j][i];
                if (currentChar != ' ')
                {
                    currentNumber += currentChar;
                }
            }

            if (currentNumber == string.Empty)
            {
                continue;
            }

            currentNumbers.Add(ulong.Parse(currentNumber));
            currentNumber = string.Empty;

            var op = operators[i];
            if (op != ' ')
            {
                var result = currentNumbers.Aggregate((a, b) => DoMath(op, a, b));
                results.Add(result);
                currentNumbers = [];
            }
        }

        var answer = results.Aggregate((a, b) => a + b);

        Console.WriteLine(answer);
    }

    private async Task Part1Async()
    {
        List<List<ulong>> numberGrid = [];
        List<string> operators = [];

        using (TextReader reader = File.OpenText("./2025/Day06.txt"))
        {
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                if (ulong.TryParse(parts[0], out var _))
                {
                    numberGrid.Add([]);
                    foreach (var part in parts)
                    {
                        numberGrid[^1].Add(ulong.Parse(part));
                    }
                }
                else
                {
                    operators.AddRange(parts);
                }
            }

            var numProblems = numberGrid[0].Count;
            var solutions = operators.Select(x => x == "*" ? 1UL : 0UL).ToList();

            for (int i = 0; i < numProblems; i++)
            {
                for (int j = 0; j < numberGrid.Count; j++)
                {
                    var number = numberGrid[j][i];

                    solutions[i] = DoMath(operators[i], solutions[i], number);
                }
            }

            var total = 0UL;
            foreach (var solution in solutions)
            {
                //Console.WriteLine(solution);
                total += solution;
            }
            Console.WriteLine(total);
        }
    }

    private ulong DoMath(string op, ulong a, ulong b)
    {
        if (op == "*")
        {
            return a * b;
        }
        else
        {
            return a + b;
        }
    }

    private ulong DoMath(char op, ulong a, ulong b)
    {
        if (op == '*')
        {
            return a * b;
        }
        else
        {
            return a + b;
        }
    }
}
