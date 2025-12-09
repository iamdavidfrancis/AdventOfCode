using AdventOfCode.Helpers.IntervalTree;

namespace AdventOfCode.AoC2025;

public class Day05 : IAsyncAdventOfCodeProblem
{
    public async Task RunProblemAsync()
    {
        using (TextReader reader = File.OpenText("./2025/Day05.txt"))
        {
            string? line;
            IntervalTree<ulong> intervalTree = [];

            // Ranges
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line == string.Empty)
                {
                    break;
                }

                var parts = line.Split('-');
                var start = ulong.Parse(parts[0]);
                var end = ulong.Parse(parts[1]);

                intervalTree.Add(start, end);
            }

            var freshCount = 0;

            // Your ingredients
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var ingredientId = ulong.Parse(line);

                if (intervalTree.Contains(ingredientId))
                {
                    freshCount++;
                }
            }

            Console.WriteLine($"Part1: {freshCount}");

            // Merge intervals
            var items = intervalTree.OrderBy(i => i.From).ToList();
            var itemCount = items.Count;

            List<RangePair<ulong>> mergedItems = [items[0]];
            for (int i = 1; i < itemCount; i++)
            {
                var last = mergedItems[^1];
                var current = items[i];

                if (current.From <= last.To)
                {
                    var merged = new RangePair<ulong>(last.From, Math.Max(last.To, current.To));
                    mergedItems[^1] = merged;
                }
                else
                {
                    mergedItems.Add(current);
                }
            }

            ulong totalCovered = 0;
            foreach (var item in mergedItems)
            {
                totalCovered += item.To - item.From + 1;
                //Console.WriteLine($"{item.From}-{item.To}: {item.To - item.From + 1} items");
            }

            Console.WriteLine($"Part2: {totalCovered}");
        }
    }
}
