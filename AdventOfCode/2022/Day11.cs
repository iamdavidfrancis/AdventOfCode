using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day11 : IAsyncAdventOfCodeProblem
    {
        private const bool ShouldLog = false;
        private const bool IsPart1 = false;

        private long globalModulo;
        
        public async Task RunProblemAsync()
        {
            List<Monkey> monkeys = new();

            using (TextReader reader = File.OpenText("./2022/Day11.txt"))
            {
                string? line;
                int monkeyId;
                Monkey monkey;
                List<string> monkeyLines = new();    

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrEmpty(line)) {
                        monkey = ParseMonkey(monkeyLines, out monkeyId);
                        monkeys.Add(monkey);
                        monkeyLines = new();
                    }
                    else
                    {
                        monkeyLines.Add(line);
                    }
                }

                // Add last monkey
                monkey = ParseMonkey(monkeyLines, out monkeyId);
                monkeys.Add(monkey);

                // Cheeky solve so I don't need BigInteger (Which takes insanely long to execute)
                // Originally this line was:
                // this.globalModulo = monkeys.Select(m => m.Value.TestDivisible).Aggregate((m,i) => m*i);
                this.globalModulo = (long)AoCMath.LCM(monkeys.Select(m => (ulong)m.TestDivisible).ToArray());

                // We can mod the worry by the Least Common Multiple of all the monkeys divisors because
                // a % LCM(b, c, d) = e => a % b == e % b, a % c == e % c, etc. 
                // Originally I didn't use the LCM, instead going with the product of all the divisors, but the LCM
                // is valid and gives us the potential for a smaller modulo (not that I expect it to be bigger than long)

                for (int i = 0; i < (IsPart1 ? 20 : 10000); i++) {
                    this.DoRound(monkeys);
                }

                for (int i = 0; i < monkeys.Count; i++)
                {
                    Log($"Monkey {i} inspected items {monkeys[i].InspectionCount} times.");
                }

                var monkeyBusiness = monkeys
                    .Select(m => m.InspectionCount)
                    .OrderByDescending(c => c)
                    .Take(2)
                    .Aggregate((a, b) => a * b);

                Console.WriteLine($"Solution: {monkeyBusiness}");
            }
        }

        private void DoRound(List<Monkey> monkeys) {
            for (int i = 0; i < monkeys.Count; i++)
            {
                this.ProcessMonkey(i, monkeys);
            }
        }

        private void ProcessMonkey(int monkeyId, List<Monkey> monkeys)
        {
            var monkey = monkeys[monkeyId];

            Log($"Monkey {monkeyId}");

            while (monkey.Items.TryDequeue(out var item)) {
                monkey.InspectionCount++;

                Log($"  Monkey inspects an item with a worry level of {item}.");

                var inspect = IsPart1
                    ? monkey.Operation(item) / 3
                    : monkey.Operation(item) % globalModulo;

                Log($"    Monkey gets bored with item. Worry level is divided by {(IsPart1 ? 3 : 1)} to {inspect}.");

                var testResult = inspect % monkey.TestDivisible == 0;
                
                int throwTo = monkey.TargetMonkeyTestPass;
                if (!testResult)
                {
                    throwTo = monkey.TargetMonkeyTestFail;
                    
                }
                
                Log($"    Item with worry level {inspect} is thrown to monkey {throwTo}.");
                monkeys[throwTo].Items.Enqueue(inspect);
            }
        }

        private Monkey ParseMonkey(List<string> MonkeyLines, out int monkeyId)
        {
            Regex pattern = new Regex(@"Monkey (?<monkeyId>\d):");
            Match match = pattern.Match(MonkeyLines[0]);
            monkeyId = Int32.Parse(match.Groups["monkeyId"].Value);

            // Items
            pattern = new Regex(@"  Starting items: (?<items>.+)");
            match = pattern.Match(MonkeyLines[1]);
            Queue<long> items  = match.Groups["items"].Value.Split(", ").Select(i => Int64.Parse(i)).ToQueue();

            // Operation
            pattern = new Regex(@"  Operation: new = old (?<operation>.+)");
            match = pattern.Match(MonkeyLines[2]);
            var operation = BuildOperation(match.Groups["operation"].Value);

            // Test
            pattern = new Regex(@"Test: divisible by (?<test>.+)");
            match = pattern.Match(MonkeyLines[3]);
            var test = Int32.Parse(match.Groups["test"].Value);

            // True Monkey
            pattern = new Regex(@"    If true: throw to monkey (?<test>.+)");
            match = pattern.Match(MonkeyLines[4]);
            var trueMonkey = Int32.Parse(match.Groups["test"].Value);

            // False Monkey
            pattern = new Regex(@"    If false: throw to monkey (?<test>.+)");
            match = pattern.Match(MonkeyLines[5]);
            var falseMonkey = Int32.Parse(match.Groups["test"].Value);

            return new Monkey(items, operation, test, trueMonkey, falseMonkey);
        }

        private Func<long, long> BuildOperation(string operation)
        {
            var parts = operation.Split(" ");
            var arithmeticOperation = parts[0];
            var value = parts[1];

            return (long old) => {
                var target = (value == "old") ? old : Int32.Parse(value);

                var newVal = arithmeticOperation switch {
                    "+" => old + target,
                    "-" => old - target,
                    "*" => old * target,
                    _ => old / target,
                };

                Log($"    Worry level is {arithmeticOperation} by {target} to {newVal}.");

                return newVal;
            };
        }

        private void Log(string message) {
            if (ShouldLog)
            {
                Console.WriteLine(message);
            }
        }

        private class Monkey
        {
            public Monkey(Queue<long> items,
                          Func<long, long> operation,
                          int test,
                          int targetPass,
                          int targetFail) 
            {
                this.Items = items;
                this.Operation = operation;
                this.TestDivisible = test;
                this.TargetMonkeyTestPass = targetPass;
                this.TargetMonkeyTestFail = targetFail;
            }

            public long InspectionCount { get; set; } = 0;

            public Queue<long> Items { get; set; } = new();

            public Func<long, long> Operation { get; set; }

            public int TestDivisible { get; set; }

            public int TargetMonkeyTestPass { get; set; }

            public int TargetMonkeyTestFail { get; set; }
        }
    }
}

