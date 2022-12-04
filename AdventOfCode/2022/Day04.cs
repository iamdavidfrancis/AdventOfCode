namespace AdventOfCode._2022
{
    internal class Day04 : IAsyncAdventOfCodeProblem
    {
        private class Range {
            public Range(string range) {
                var pairs = range.Split("-");
                this.Lower = Convert.ToInt32(pairs[0]);
                this.Upper = Convert.ToInt32(pairs[1]);
            }
            public int Lower { get; set; }
            public int Upper { get; set; }
        }
        public async Task RunProblemAsync()
        {
            // Part 1
            using (TextReader reader = File.OpenText("./2022/Day04.txt"))
            {
                string? line;
                int count = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                { 
                    var pairs = line.Split(",");
                    var left = new Range(pairs[0]);
                    var right = new Range(pairs[1]);

                    if ((left.Lower <= right.Lower && left.Upper >= right.Upper) ||
                        (right.Lower <= left.Lower && right.Upper >= left.Upper))
                    {
                        count++;
                    }
                }

                Console.WriteLine($"Part 1: {count}");
            }

            // Part 2
            using (TextReader reader = File.OpenText("./2022/Day04.txt"))
            {
                string? line;
                int count = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                { 
                    var pairs = line.Split(",");
                    var left = new Range(pairs[0]);
                    var right = new Range(pairs[1]);

                    if (!(left.Lower > right.Upper || left.Upper < right.Lower))
                    {
                        count++;
                    }
                }

                Console.WriteLine($"Part 2: {count}");
            }            
        }
    }
}