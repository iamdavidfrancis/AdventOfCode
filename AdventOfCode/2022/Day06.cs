using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day06 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2022/Day06.txt"))
            {
                string? line = await reader.ReadLineAsync();
                var letters = line!.ToCharArray();

                var answer1 = SolveInput(4, letters);
                var answer2 = SolveInput(14, letters);

                Console.WriteLine($"Part 1: {answer1}");
                Console.WriteLine($"Part 2: {answer2}");
            }
        }

        private static int SolveInput(int size, char[] letters) {
            int answer = size;

            var currentLetters = new Queue<char>(letters.Take(size));
            var remainingLetters = new Queue<char>(letters.Skip(size));

            do {
                if (currentLetters.Distinct().Count() == size) {
                    break;                            
                }

                currentLetters.Dequeue();
                currentLetters.Enqueue(remainingLetters.Dequeue());
                answer++;
            } while (remainingLetters.Any());

            return answer;
        }
    }
}