
namespace AdventOfCode;

public abstract class AdventBase : IAsyncAdventOfCodeProblem
{
    public abstract string FilePath { get; }

    public async Task RunProblemAsync()
    {
        using (TextReader reader = File.OpenText(this.FilePath))
        {
            var result = await RunInternal(reader);

            Console.WriteLine("Part 1");
            Console.WriteLine(result.Part1);

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(result.Part2);
        }
    }

    public abstract Task<AdventResult> RunInternal(TextReader reader);
}

public record AdventResult(string Part1, string Part2)
{
    public AdventResult(int Part1, int Part2)
        : this(Part1.ToString(), Part2.ToString())
    {
    }

    public AdventResult(long Part1, long Part2)
        : this(Part1.ToString(), Part2.ToString())
    {
    }
}