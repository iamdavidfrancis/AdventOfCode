using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day05 : IAsyncAdventOfCodeProblem
    {
        // Note: For this, I pre-processed the input slightly.
        public async Task RunProblemAsync()
        {
            // Part 1
            using (TextReader reader = File.OpenText("./2022/Day05.Parsed.txt"))
            {
                string? line;
                List<Stack<char>> crateStacks = new();
                List<Stack<char>> crateStacks2 = new();

                // Build Map
                while (!string.IsNullOrWhiteSpace((line = await reader.ReadLineAsync())))
                {
                    Stack<char> crates = new();
                    Stack<char> crates2 = new();

                    foreach (var letter in line!) {
                        crates.Push(letter);
                        crates2.Push(letter);
                    }

                    crateStacks.Add(crates);
                    crateStacks2.Add(crates2);
                }

                string pat = "move (.+?) from (.+?) to (.+)$";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);

                while ((line = await reader.ReadLineAsync()) != null)
                { 
                    var match = r.Match(line);

                    var toMove = Int32.Parse(match.Groups[1].ToString());
                    var fromCol = Int32.Parse(match.Groups[2].ToString());
                    var toCol = Int32.Parse(match.Groups[3].ToString());

                    var from = crateStacks[fromCol - 1];
                    var to = crateStacks[toCol - 1];

                    var from2 = crateStacks2[fromCol - 1];
                    var to2 = crateStacks2[toCol - 1];

                    var temp = new Stack<char>();

                    for (int i = 0; i < toMove; i++) {
                        to.Push(from.Pop());
                        temp.Push(from2.Pop());
                    }

                    for (int j = 0; j < toMove; j++) {
                        to2.Push(temp.Pop());
                    }
                }

                Console.Write($"Part 1:");
                
                foreach (var stack in crateStacks) {
                    Console.Write(stack.First());
                }

                Console.WriteLine();

                Console.Write($"Part 2:");
                
                foreach (var stack in crateStacks2) {
                    Console.Write(stack.First());
                }

                Console.WriteLine();
            }
        }
    }
}