using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdventOfCode._2023;

public class Day09 : AdventBase
{
    public override string FilePath => "./2023/Day09.txt";

    public override async Task<AdventResult> RunInternal(TextReader reader)
    {
        int answer1 = 0;
        int answer2 = 0;
        string? line;
        
        while ((line = await reader.ReadLineAsync()) != null) 
        {
            var seq = line
                .Split(" ")
                .Select(c => int.Parse(c))
                .ToList();

            List<List<int>> sequences = [seq];

            // Build patterns
            while (sequences[^1].Any(c => c != 0))
            {
                var nextSeq = new List<int>();
                var curr = sequences[^1];

                for (int i = 1; i < curr.Count; ++i)
                {
                    nextSeq.Add(curr[i] - curr[i - 1]);
                }

                sequences.Add(nextSeq);
            }

            // Process back up to top
            int pattern1 = 0;
            int pattern2 = 0;
            for (int i = sequences.Count - 1; i >= 0; i--)
            {
                var lastSeq = sequences[i];

                var currentEnd = lastSeq[^1];
                var currentStart = lastSeq[0];

                pattern1 += currentEnd;
                pattern2 = currentStart - pattern2;
                lastSeq.Add(pattern1);
                lastSeq.Insert(0, pattern2);
            }

            answer1 += sequences[0][^1];
            answer2 += sequences[0][0];
        }

        return new(answer1, answer2);
    }
}