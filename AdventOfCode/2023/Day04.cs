using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023;
using Card = (HashSet<string> winningNums, IEnumerable<string> pickedNums);

public class Day04 : IAsyncAdventOfCodeProblem
{
    
    public async Task RunProblemAsync()
    {
        using (TextReader reader = File.OpenText("./2023/Day04.txt"))
        {
            string? line;
            uint sum1 = 0;
            uint sum2 = 0;

            Dictionary<int, Card> cards = new();
            Queue<int> processQueue = new();
            
            while ((line = await reader.ReadLineAsync()) != null) 
            {
                var matches = Regex.Match(line, @"Card +?(\d+?): +([0-9 ]+?) \| +([0-9 ]+?)$");
                var cardNum = int.Parse(matches.Groups[1].Value);
                var winningNumString = matches.Groups[2].Value;
                var pickedNumString = matches.Groups[3].Value;

                var winningNums = winningNumString.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet();
                var pickedNums = pickedNumString.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                sum1 += ProcessCard(winningNums, pickedNums).sum;

                cards[cardNum] = (winningNums, pickedNums);
                processQueue.Enqueue(cardNum);
            }

            while (processQueue.Any())
            {
                var cardNum = processQueue.Dequeue();
                var card = cards[cardNum];
                sum2++;

                var numWins = ProcessCard(card.winningNums, card.pickedNums).numWins;

                if (numWins > 0)
                {
                    for (int i = 1; i <= numWins; i++)
                    {
                        int nextCard = cardNum + i;
                        if (nextCard > cards.Count) {
                            break;
                        }

                        processQueue.Enqueue(nextCard);
                    }
                }
            }

            Console.WriteLine("Part 1");
            Console.WriteLine(sum1);

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(sum2);
        }
    }

    private static (uint sum, int numWins) ProcessCard(HashSet<string> winningNums, IEnumerable<string> pickedNums)
    {
        int numWinners = 0;

        foreach (var picked in pickedNums)
        {
            if (winningNums.Contains(picked))
            {
                numWinners++;
                if (numWinners == winningNums.Count) {
                    break;
                }
            }
        }

        if (numWinners > 0)
        {
            return ((uint)1 << (numWinners - 1), numWinners);
        }

        return (0, 0);
    }
}