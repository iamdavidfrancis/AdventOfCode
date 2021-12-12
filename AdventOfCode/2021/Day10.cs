using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day10 : IAsyncAdventOfCodeProblem
    {
        private readonly char[] ClosingChars = new char[] { ')', ']', '}', '>' };
        public async Task RunProblemAsync()
        {
            List<List<int>> heightMap = new List<List<int>>();

            using (TextReader reader = File.OpenText("./2021/Day10Input.txt"))
            {
                // int sum = 0;
                List<ulong> lineScores = new();

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // Part 1
                    // sum += ProcessLine(line).corruptionScore;

                    var (isCorrupted, expected, _) = ProcessLine(line);

                    // Part 2
                    if (isCorrupted)
                    {
                        continue;
                    }

                    ulong lineScore = 0;

                    while (expected.Any())
                    {
                        lineScore *= 5;

                        switch (expected.Pop())
                        {
                            case ')':
                                lineScore += 1;
                                break;
                            case ']':
                                lineScore += 2;
                                break;
                            case '}':
                                lineScore += 3;
                                break;
                            default:
                                lineScore += 4;
                                break;
                        }
                    }

                    lineScores.Add(lineScore);
                }

                lineScores.Sort();

                var middle = lineScores.Count / 2;

                Console.WriteLine(lineScores[middle]);
            }
        }

        private (bool isCorrupted, Stack<char> expected, int corruptionScore) ProcessLine(string line)
        {
            Stack<char> expected = new();

            int idx = 0;
            foreach (char c in line)
            {
                if (IsClosing(c))
                {
                    if (!expected.TryPop(out char expectedChar) || c != expectedChar)
                    {
                        // Console.WriteLine($"{line}\tExpected {expectedChar}, but found {c} at position {idx}");
                        return (true, expected, ScoreCorruption(c));
                    }
                }
                else
                {
                    switch (c)
                    {
                        case '(':
                            expected.Push(')');
                            break;
                        case '[':
                            expected.Push(']');
                            break;
                        case '{':
                            expected.Push('}');
                            break;
                        default:
                            expected.Push('>');
                            break;
                    }
                }

                idx++;
            }

            return (false, expected, 0);
        }

        private bool IsClosing(char input)
        {
            return ClosingChars.Contains(input);
        }

        private int ScoreCorruption(char input)
        {
            switch (input)
            {
                case ')':
                    return 3;
                case ']':
                    return 57;
                case '}':
                    return 1197;
                default:
                    return 25137;
            }
        }
    }
}
