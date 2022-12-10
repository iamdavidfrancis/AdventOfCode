using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day10 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2022/Day10.txt"))
            {
                string? line;

                Queue<Operation> operations = new();
                Queue<Operation> operations2 = new();

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var parts = line.Split(" ");
                    var opcode = parts[0];

                    if (opcode == "noop")
                    {
                        operations.Enqueue(new Operation { OperationType = OperationType.noop });
                        operations2.Enqueue(new Operation { OperationType = OperationType.noop });
                    }
                    else 
                    {
                        operations.Enqueue(new Operation { OperationType = OperationType.addx, Value = Int32.Parse(parts[1]) });
                        operations2.Enqueue(new Operation { OperationType = OperationType.addx, Value = Int32.Parse(parts[1]) });
                    }
                }

                // Part 1
                var strengths = GetScores(operations, true);
                var intervals = new List<int> { 20, 60, 100, 140, 180, 220 };
                var part1 = strengths.Where(c =>  intervals.Contains(c.cycle)).Sum(c => c.signalStrength);

                Console.WriteLine($"Part 1: {part1}");
                Console.WriteLine($"Part 2:");

                // Part 2
                var strengths2 = GetScores(operations2);
                
                foreach (var item in strengths2) 
                {
                    var pos = item.cycle % 40;

                    if (Math.Abs(item.x - pos) <= 1)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }

                    if (pos == 39) {
                        Console.WriteLine();
                    }
                }
            }
        }

        private IEnumerable<Result> GetScores(Queue<Operation> operations, bool part1 = false) {
            int clock = part1 ? 1 : 0;
            int x = 1;
            
            yield return new Result(clock, x * clock, x);

            while (operations.Any()) 
            {
                var operation = operations.Dequeue();

                switch (operation.OperationType) {
                    case OperationType.noop:
                        clock++;
                        break;
                    case OperationType.addx:
                        clock++;
                        yield return new Result(clock, x * clock, x);
                        clock++;
                        x += operation.Value;
                        break;
                }
                
                yield return new Result(clock, x * clock, x);
            }
        }

        private record Result(int cycle, int signalStrength, int x);

        private class Operation {
            public OperationType OperationType { get; set; }
            public int Value { get; set; }
        }

        private enum OperationType {
            addx,
            noop
        }
    }
}

