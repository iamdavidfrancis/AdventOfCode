
using System.Text;

namespace AdventOfCode.AoC2025;

public class Day01 : IAsyncAdventOfCodeProblem
{
    public async Task RunProblemAsync()
    {
        var dial = new Dial(50);

        int password = 0;
        
        using (TextReader reader = File.OpenText("./2025/Day01.txt"))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null) 
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var crossedZero = dial.Rotate(line);

                password += crossedZero;

                if (dial.Position == 0)
                {
                    password++;
                }
            }
        }
        
        Console.WriteLine($"Final position: {dial.Position}");
        Console.WriteLine($"Password: {password}");
    }
}
