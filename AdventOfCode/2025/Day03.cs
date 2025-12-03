namespace AdventOfCode.AoC2025;

public class Day03 : IAsyncAdventOfCodeProblem
{
    public async Task RunProblemAsync()
    {
               
        using (TextReader reader = File.OpenText("./2025/Day03.txt"))
        {
            string? line;
            ulong acc = 0;
            while ((line = await reader.ReadLineAsync()) != null) 
            {
                var bestJoltage = GetBestJoltage(line, 12);
                Console.WriteLine($"Best joltage for line {line} is {bestJoltage}");
                acc += bestJoltage;
            }
            Console.WriteLine($"Accumulated joltage: {acc}");
        }
    }

    private ulong GetBestJoltage(string line, int numBatteries = 2)
    {
        var numbers = line.Select(c => ulong.Parse(c.ToString())).ToList();
        // Console.WriteLine(string.Join(", ", numbers));
        ulong acc = 0;

        for (int i = numBatteries; i > 0; i--)
        {
            acc *= 10;

            var allButLastN = numbers.Take(numbers.Count - i + 1).ToList();
            var (bestValue, bestIndex) = FindBestOption(allButLastN);
            acc += (ulong)bestValue;

            numbers = numbers.Skip(bestIndex + 1).ToList();
        }

        return acc;
    }

    private (ulong Value, int Index) FindBestOption(List<ulong> options)
    {
        var bestValue = options.Max();
        var bestIndex = options.IndexOf(bestValue);
        return (bestValue, bestIndex);
    }
}
