
using System.Text;

namespace AdventOfCode._2023;

public class Day01 : IAsyncAdventOfCodeProblem
{
    private Dictionary<string, int> NumberMap = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    public async Task RunProblemAsync()
    {
        using (TextReader reader = File.OpenText("./2023/Day01.txt"))
        {
            string? line;
            int sum = 0;
            int sum2 = 0;

            while ((line = await reader.ReadLineAsync()) != null) 
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                // Part 1
                var nums = line.Where(char.IsDigit);
                var numString = string.Concat(nums.First(), nums.Last());

                if (int.TryParse(numString, out int result))
                {
                    sum += result;
                }

                // Part 2
                // Preprocess the lines
                var val = FindNum(line) * 10 + FindNum(line, true);
                sum2 += val;
            }

            Console.WriteLine("Part 1");
            Console.WriteLine(sum);

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(sum2);
        }
    }

    private int FindNum(string line, bool fromRight = false)
    {
        Func<int, int> processItem = (i) =>
        {
            if (char.IsDigit(line[i]))
            {
                return int.Parse(line[i].ToString());
            }

            var subLine = line.Substring(i);
            var match = NumberMap.Keys.SingleOrDefault(k => subLine.StartsWith(k));
            if (match != null)
            {
                return NumberMap[match];
            }

            return -1;
        };

        if (!fromRight)
        {
            for (int i = 0; i < line.Length; i++)
            {
                var res = processItem(i);

                if (res >= 0)
                {
                    return res;
                }
            }
        }
        else
        {
            for (int i = line.Length - 1; i >= 0; i--)
            {
                var res = processItem(i);

                if (res >= 0)
                {
                    return res;
                }
            }
        }

        throw new Exception("Something went wrong.");
        
    }
}