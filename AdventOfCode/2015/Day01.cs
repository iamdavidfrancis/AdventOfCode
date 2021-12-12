using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2015
{
    internal class Day01 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2015/Day01Input.txt"))
            {
                int currentFloor = 0;

                string? line = await reader.ReadLineAsync();

                if (line == null)
                {
                    return;
                }

                var idx = 1;
                foreach (var instruction in line)
                {
                    if (instruction == '(')
                    {
                        currentFloor++;
                    }
                    else
                    {
                        currentFloor--;
                    }

                    // Part 2 only
                    if (currentFloor == -1)
                    {
                        break;
                    }

                    idx++;
                }

                // Part 1
                // Console.WriteLine(currentFloor);
                
                // Part 2
                Console.WriteLine(idx);
            }
        }
    }
}
