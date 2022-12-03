namespace AdventOfCode._2022
{
    internal class Day03 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            // Part 1
            // using (TextReader reader = File.OpenText("./2022/Day03.txt"))
            // {
            //     string? line;
            //     int score = 0;

            //     while ((line = await reader.ReadLineAsync()) != null)
            //     { 
            //         var mid = line.Length / 2;
            //         var left = line.Take(mid);
            //         var right = line.Skip(mid);

            //         var sol = left.Intersect(right);

            //         score += sol.Select(c => GetVal(c)).Sum();
            //     }

            //     Console.WriteLine(score);
            // }

            // Part 2
            using (TextReader reader = File.OpenText("./2022/Day03.txt"))
            {
                string? line;
                List<string> groupList = new List<string>();
                
                int score = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                { 
                    groupList.Add(line);

                    if (groupList.Count == 3)
                    {
                        var badge = groupList[0].Intersect(groupList[1]).Intersect(groupList[2]);
                        score += GetVal(badge.Single());

                        groupList.Clear();
                    }
                }

                Console.WriteLine(score);
            }            
        }

        private int GetVal(char input) {
            return (input & 32) == 32 ? ((byte)input - 96) : ((byte)input - 38);
        }
    }
}