namespace AdventOfCode._2023;

using System.Globalization;
using Map = List<MapRow>;

public class Day05 : IAsyncAdventOfCodeProblem
{
    public async Task RunProblemAsync()
    {
        List<ulong> seeds1 = [];
        List<ulong> seeds2 = [];

        using (TextReader reader = File.OpenText("./2023/Day05.txt"))
        {
            // Fetch seeds
            var seedString = await reader.ReadLineAsync();

            // Read blank line to prime the rest of this.
            await reader.ReadLineAsync();

            var seedVals = seedString!.Split(": ")[1];
            var seedParts = seedVals.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (int i = 0; i < seedParts.Length; i += 2)
            {
                var startSeed = ulong.Parse(seedParts[i]);
                var seedCount = ulong.Parse(seedParts[i + 1]);

                // Part 1
                seeds1.Add(startSeed);
                seeds1.Add(seedCount);

                // Part 2
                Console.WriteLine($"Adding {startSeed + seedCount} items for Part2");
                for (ulong j = startSeed; j < startSeed + seedCount; j++)
                {
                    seeds2.Add(j);
                }
            }

            Console.WriteLine(seeds2.Count);

            for (int i = 0; i < 7; i++)
            {
                var map = await GetNextMap(reader);
                ProcessMap(map, seeds1);
                ProcessMap(map, seeds2);
            }
            
            Console.WriteLine("Part 1");
            Console.WriteLine(seeds1.Min());

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(seeds2.Min());
        }
    }

    private void ProcessMap(Map map, IList<ulong> seeds)
    {
        int size = seeds.Count;
        for (int i = 0; i < size; i++)
        {
            var seed = seeds[i];
            foreach (var mapItem in map)
            {
                if (seed >= mapItem.SourceStart && seed < mapItem.SourceStart + mapItem.Size)
                {
                    seeds[i] = mapItem.DestStart + (seed - mapItem.SourceStart);
                    // Console.WriteLine($"Mapping: {seed} to {seeds[i]}");
                }
            }
        }
    }

    private async Task<Map> GetNextMap(TextReader reader)
    {
        Map map = new();

        // Read empty line and title
        await reader.ReadLineAsync();

        string? line;

        while ((line = await reader.ReadLineAsync()) != "") 
        {
            if (line is null)
            {
                break;
            }

            var parts = line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            map.Add(new MapRow(parts[0], parts[1], parts[2]));
        }

        return map;
    }

}

public record MapRow(ulong DestStart, ulong SourceStart, ulong Size)
{
    public MapRow(string DestStart, string SourceStart, string Size)
        : this(ulong.Parse(DestStart), ulong.Parse(SourceStart), ulong.Parse(Size))
    {
    }
}