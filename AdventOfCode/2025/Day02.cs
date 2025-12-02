using System.Xml.Schema;

namespace AdventOfCode.AoC2025;

public class Day02 : IAsyncAdventOfCodeProblem
{
    public async Task RunProblemAsync()
    {
        List<Range> ranges;
        using (TextReader reader = File.OpenText("./2025/Day02.txt"))
        {
            var line = await reader.ReadLineAsync();

            ranges = line?.Split(',')
                .Select(part =>
                {
                    var bounds = part.Split('-');
                    return new Range(ulong.Parse(bounds[0]), ulong.Parse(bounds[1]));
                })
                .ToList() ?? [];
        }

        List<ulong> invalidIds = new();
        foreach (var range in ranges)
        {
            Console.WriteLine($"Range: {range.Start}-{range.End}");

            for (ulong id = range.Start; id <= range.End; id++)
            {
                // Console.WriteLine($"Testing {id}");
                if (!IsValidId(id))
                {
                    // Console.WriteLine($"  Invalid ID: {id}");
                    invalidIds.Add(id);
                }
            }
        }

        Console.WriteLine(invalidIds.Aggregate((a, b) => a + b));
    }

    private bool IsValidId(ulong id)
    {
        var digits = id.ToString().Length;

        // Odd digits can't be a repeating pair
        // if (digits % 2 != 0)
        // {
        //     return true;
        // }

        var midPoint  = digits / 2;

        for (int i = 1; i <= midPoint; i++)
        {
            var seq = id.ToString().Substring(0, i);

            // Console.WriteLine($"  Testing seq: {seq}");

            if (digits % seq.Length != 0)
            {
                // Console.WriteLine("    Skipping due to length mismatch");
                continue;
            }

            var repeatedSeq = string.Concat(Enumerable.Repeat(seq, digits / seq.Length));
            // Console.WriteLine($"  Testing repeatedSeq: {repeatedSeq}");
            if (repeatedSeq == id.ToString())
            {
                return false;
            }
        }

        return true;
    }

    private record Range(ulong Start, ulong End);
}
