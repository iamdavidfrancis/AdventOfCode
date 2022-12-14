using System.Text.Json;

namespace AdventOfCode._2022
{
    // Part 1
    public class Day13 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {

            List<(JsonDocument left, JsonDocument right)> pairs = new();
            List<JsonDocument> packets = new();
            packets.Add(JsonDocument.Parse("[[2]]"));
            packets.Add(JsonDocument.Parse("[[6]]"));

            using (TextReader reader = File.OpenText("./2022/Day13.txt"))
            {
                string? line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) {
                        continue;
                    }

                    var line2 = await reader.ReadLineAsync();

                    var left = JsonDocument.Parse(line);
                    var right = JsonDocument.Parse(line2!);

                    pairs.Add((left, right));
                    packets.Add(left);
                    packets.Add(right);
                }
            }

            var idx = 1;
            var sum = 0;
            foreach (var (left, right) in pairs)
            {
                var result = CompareItems(left.RootElement, right.RootElement, 0, true);

                if (result == CompareResult.CorrectOrder)
                {
                    sum += idx;
                }

                idx++;
            }

            Console.WriteLine($"Part 1: {sum}");

            packets.Sort((JsonDocument left, JsonDocument right) => {
                var result = CompareItems(left.RootElement, right.RootElement, 0, true);
                return result switch {
                    CompareResult.CorrectOrder => -1,
                    CompareResult.IncorrectOrder => 1,
                    CompareResult.Continue => 0,
                };                
            });

            var result2 = 1;
            for (int i = 0; i < packets.Count; i++)
            {
                if (packets[i].RootElement.ToString() == "[[2]]" || packets[i].RootElement.ToString() == "[[6]]")
                {
                    result2 *= i + 1;
                }
            }

            Console.WriteLine($"Part 2: {result2}");
        }

        private CompareResult CompareItems(JsonElement left, JsonElement right, int indent, bool isTopLevel = false)
        {
            if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
            {
                return CompareInts(left.GetInt32(), right.GetInt32(), indent + 1);
            }
            else if (left.ValueKind == JsonValueKind.Array && right.ValueKind == JsonValueKind.Array)
            {
                var leftSize = left.GetArrayLength();
                var rightSize = right.GetArrayLength();
                var maxScan = (leftSize < rightSize) ? leftSize : rightSize;
                
                var result = CompareResult.Continue;

                for (int i = 0; i < maxScan; i++)
                {
                    result = CompareItems(left[i], right[i], indent + 1);

                    if (result != CompareResult.Continue)
                    { 
                        return result;
                    }
                }

                // We ran out of length
                if (leftSize != rightSize)
                {
                    return (leftSize > rightSize) ? CompareResult.IncorrectOrder : CompareResult.CorrectOrder;
                }
                else
                {
                    return CompareResult.Continue;
                }
                
            }
            else if (left.ValueKind == JsonValueKind.Number)
            {
                var newLeft = JsonDocument.Parse($"[{left.GetInt32()}]").RootElement;

                return CompareItems(newLeft, right, indent + 1);
            }
            else
            {
                var newRight = JsonDocument.Parse($"[{right.GetInt32()}]").RootElement;

                return CompareItems(left, newRight, indent + 1);
            }
        }

        private CompareResult CompareInts(int left, int right, int indent)
        {
            if (left < right)
            {
                return CompareResult.CorrectOrder;
            }
            else if (left > right)
            {
                return CompareResult.IncorrectOrder;
            }
            else
            {
                return CompareResult.Continue;
            }
        }

        private enum CompareResult
        {
            CorrectOrder,
            IncorrectOrder,
            Continue,
        }
    }
}

