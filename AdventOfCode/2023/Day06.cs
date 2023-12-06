namespace AdventOfCode._2023;

public class Day06 : IAsyncAdventOfCodeProblem
{

    public async Task RunProblemAsync()
    {
        ulong part1 = 1;

        using (TextReader reader = File.OpenText("./2023/Day06.txt"))
        {
            string? timesString = (await reader.ReadLineAsync())!.Split(":")[1].Trim();
            string? distancesString = (await reader.ReadLineAsync())!.Split(":")[1].Trim();

            var times = timesString.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(c => ulong.Parse(c));
            var distances = distancesString.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(c => ulong.Parse(c));

            var races = times.Zip(distances);

            foreach (var race in races)
            {
                var res = ProcessRace(race.First, race.Second);
                part1 *= res;
            }

            var part2 = ProcessRace(ulong.Parse(timesString.Replace(" ", string.Empty)), ulong.Parse(distancesString.Replace(" ", string.Empty)));

            Console.WriteLine("Part 1");
            Console.WriteLine(part1);

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(part2);
        }
    }

    private ulong ProcessRace(ulong time, ulong distance)
    {
        ulong count = 0;
        for (ulong i = 0; i < time; ++i)
        {
            var speed = i;
            var remainingTime = time - i;

            var currDistance = speed * remainingTime;

            if (currDistance > distance)
            {
                count++;
            }
            
        }

        return count;
    }
}