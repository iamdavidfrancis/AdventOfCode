using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2021
{
    internal class Day2 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            int forward = 0;
            int aim = 0;
            int depth = 0;

            using (TextReader reader = File.OpenText("./2021/Day2Input.txt"))
            {
                string? line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var parts = line.Split(' ');
                    var direction = parts[0];
                    var amount = Convert.ToInt32(parts[1]);

                    switch (direction)
                    {
                        case "up":
                            aim -= amount;
                            break;
                        case "down":
                            aim += amount;
                            break;
                        default:
                            forward += amount;
                            depth += amount * aim;
                            break;
                    }
                }
            }

            Console.WriteLine(forward * depth);
        }

        
    }
}
