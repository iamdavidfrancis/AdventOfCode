namespace AdventOfCode._2022
{
    internal class Day01 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2022/Day01.txt"))
            {
                string? line;

                int elfWithMostFood = 0;
                int maxFood = 0;

                List<int> Elves = new List<int>();
                int currentElf = 0;

                while ((line = await reader.ReadLineAsync()) != null) 
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Elves.Add(currentElf);

                        if (currentElf > maxFood) {
                            maxFood = currentElf;
                            elfWithMostFood = Elves.Count;
                        }
                        
                        currentElf = 0;

                        continue;
                    }

                    int lineVal = Convert.ToInt32(line);
                    currentElf += lineVal;
                }

                // Part 1.
                // Console.WriteLine($"Elf {elfWithMostFood} has {maxFood}");

                // Part 2.
                Console.WriteLine($"{Elves.OrderByDescending(c => c).Take(3).Sum()}");
            }
        }
    }
}