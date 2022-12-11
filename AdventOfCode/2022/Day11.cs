using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day11 : IAsyncAdventOfCodeProblem
    {
        private const bool ShouldLog = false;
        private const bool IsPart1 = false;

        private int globalModulo;
        
        public async Task RunProblemAsync()
        {
            // I could have used a list but I was tired. Sue me.
            Dictionary<int, Monkey> monkeys = new();

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
                        monkeys.Add(monkeyId, monkey);
                        monkeyLines = new();
                    }
                    else
                    {
                        monkeyLines.Add(line);
                    }
                }

                // Add last monkey
                monkey = ParseMonkey(monkeyLines, out monkeyId);
                monkeys.Add(monkeyId, monkey);
                monkeyLines = new();

                // Cheeky solve so I don't need BigInteger (Which takes insanely long to execute)
                this.globalModulo = monkeys.Select(m => m.Value.TestDivisible).Aggregate((m,i) => m*i);

                for (int i = 0; i < (IsPart1 ? 20 : 10000); i++) {
                    this.DoRound(monkeys);
                }

                for (int i = 0; i < monkeys.Count; i++)
                {
                    Log($"Monkey {i} inspected items {monkeys[i].InspectionCount} times.");
                }

                var monkeyBusiness = monkeys.Values.Select(m => m.InspectionCount).OrderByDescending(c => c).Take(2).Aggregate((a, b) => a * b);

                Console.WriteLine($"Solution: {monkeyBusiness}");
            }
        }

        private void DoRound(Dictionary<int, Monkey> monkeys) {
            for (int i = 0; i < monkeys.Count; i++)
            {
                this.ProcessMonkey(i, monkeys);
            }
        }

        private void ProcessMonkey(int monkeyId, Dictionary<int, Monkey> monkeys)
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
            Regex monkeyIdPattern = new Regex(@"Monkey (?<monkeyId>\d):");
            Match monkeyIdMatch = monkeyIdPattern.Match(MonkeyLines[0]);
            monkeyId = Int32.Parse(monkeyIdMatch.Groups["monkeyId"].Value);

            // Items
            Regex itemsPattern = new Regex(@"  Starting items: (?<items>.+)");
            Match itemsMatch = itemsPattern.Match(MonkeyLines[1]);
            string itemsString = itemsMatch.Groups["items"].Value;
            Queue<long> items  = itemsString.Split(", ").Select(i => Int64.Parse(i)).ToQueue();

            // Operation
            Regex operationPattern = new Regex(@"  Operation: new = old (?<operation>.+)");
            Match operationMatch = operationPattern.Match(MonkeyLines[2]);
            string operationString = operationMatch.Groups["operation"].Value;
            var operation = BuildOperation(operationString);

            // Test
            Regex testPattern = new Regex(@"Test: divisible by (?<test>.+)");
            Match testMatch = testPattern.Match(MonkeyLines[3]);
            string testString = testMatch.Groups["test"].Value;
            var test = Int32.Parse(testString);

            // True Monkey
            Regex truePattern = new Regex(@"    If true: throw to monkey (?<test>.+)");
            Match trueMatch = truePattern.Match(MonkeyLines[4]);
            var trueMonkey = Int32.Parse(trueMatch.Groups["test"].Value);

            // False Monkey
            Regex falsePattern = new Regex(@"    If false: throw to monkey (?<test>.+)");
            Match falseMatch = falsePattern.Match(MonkeyLines[5]);
            var falseMonkey = Int32.Parse(falseMatch.Groups["test"].Value);

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

