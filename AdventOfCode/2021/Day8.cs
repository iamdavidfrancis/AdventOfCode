using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day8 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2021/Day8Input.txt"))
            {
                string? line;
                var sum = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var segments = line.Split(" | ");
                    var leftSide = segments[0];
                    var rightSide = segments[1];

                    var inputSegments = leftSide.Split(' ');
                    var outputSegments = rightSide.Split(' ');

                    sum += Part2(inputSegments, outputSegments);
                }

                Console.WriteLine(sum);
            }

        }

        private int Part1(string[] segments)
        {
            return segments.Where(s => (new int[] { 2, 3, 4, 7 }).Contains(s.Length)).Count();
        }

        private int Part2(string[] inputs, string[] outputs)
        {
            // Determine each digit
            var sortedInputs = inputs.OrderBy(s => s.Length);

            Dictionary<string, int> mappedOutputs = new();
            Dictionary<int, string> reverseMap = new();

            var one = inputs.Where(s => s.Length == 2).Single();
            var seven = inputs.Where(s => s.Length == 3).Single();
            var four = inputs.Where(s => s.Length == 4).Single();
            var eight = inputs.Where(s => s.Length == 7).Single();

            mappedOutputs[Sort(one)] = 1;
            mappedOutputs[Sort(seven)] = 7;
            mappedOutputs[Sort(four)] = 4;
            mappedOutputs[Sort(eight)] = 8;

            var oneArray = one.ToCharArray();
            var fourArray = four.ToCharArray();

            var lengthSix = inputs.Where(s => s.Length == 6);

            var six = lengthSix.Where(s => !s.Contains(oneArray[0]) || !s.Contains(oneArray[1])).Single();

            lengthSix = lengthSix.Where(s => s != six);

            var nine = lengthSix.Where(s => s.Contains(fourArray[0]) && s.Contains(fourArray[1]) && s.Contains(fourArray[2]) && s.Contains(fourArray[3])).Single();

            var zero = lengthSix.Where(s => s != nine).Single();

            mappedOutputs[Sort(six)] = 6;
            mappedOutputs[Sort(nine)] = 9;
            mappedOutputs[Sort(zero)] = 0;

            var lengthFive = inputs.Where(s => s.Length == 5);

            var five = lengthFive.Where(s => TestFiveLength(s, six)).Single();

            lengthFive = lengthFive.Where(s => s != five);

            var three = lengthFive.Where(s => TestFiveLength(s, nine)).Single();

            var two = lengthFive.Where(s => s != three).Single();

            mappedOutputs[Sort(five)] = 5;
            mappedOutputs[Sort(three)] = 3;
            mappedOutputs[Sort(two)] = 2;

            // Determine output digits
            var solved = outputs.Select(s => mappedOutputs[Sort(s)]);
            var total = solved.Aggregate(0, (acc, x) => (acc * 10) + x);

            return total;
        }

        private bool TestFiveLength(string test, string six)
        {
            var testChars = test.ToCharArray();

            var matchingSegments = 0;

            foreach (var testChar in testChars)
            {
                if (six.Contains(testChar))
                {
                    matchingSegments++;
                }
            }

            return matchingSegments == 5;

        }

        private string Sort(string input)
        {
            var sorted = input.OrderBy(c => c).ToArray();

            return new string(sorted);
        }
    }
}
