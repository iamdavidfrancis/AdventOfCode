using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace AdventOfCode._2021
{
    internal class Day16 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<string> lines = new List<string>();

            using (TextReader reader = File.OpenText("./2021/Day16Input.txt"))
            {
                string? l;
                while ((l = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(l);
                }
            }

            var line = lines.Single();

            var bits = HexToBinary(line);

            Reverse(bits); // Make things a little easier.

            var (sum, length, value) = ProcessPacket(bits);

            Console.WriteLine(value);
        }

        private (int sumOfVersions, int length, int value) ProcessPacket(BitArray bits)
        {
            var mask3Bits = new BitArray(3, true);
            mask3Bits.Length = bits.Length;

            var versionBits = new BitArray(bits).And(mask3Bits);
            versionBits.Length = 3;
            var version = BitsToInt(versionBits);

            bits.RightShift(3);

            var typeIdBits = new BitArray(bits).And(mask3Bits);
            typeIdBits.Length = 3;
            var typeId = BitsToInt(typeIdBits);

            bits.RightShift(3);

            var length = 6;
            var versionSum = version;

            var value = 0;

            if (typeId == 4)
            {
                // Literal Packet
                var (literalPacketValue, literalLength) = ProcessLiteralPacket(bits);

                value += literalPacketValue;
                length += literalLength;
            }
            else
            {
                var lengthTypeId = bits[0];
                bits.RightShift(1);

                Func<List<int>, int> subPacketsOperation = typeId switch
                {
                    0 => (List<int> values) => values.Aggregate(0, (acc, val) => acc + val), // Sum
                    1 => (List<int> values) => values.Aggregate(1, (acc, val) => acc * val), // Product
                    2 => (List<int> values) => values.Min(), // Minimum
                    3 => (List<int> values) => values.Max(), // Maximum
                    5 => (List<int> values) => values[0] > values[1] ? 1 : 0, // Greater Than
                    6 => (List<int> values) => values[0] < values[1] ? 1 : 0, // Less Than
                    _ => (List<int> values) => values[0] == values[1] ? 1 : 0, // Equal To
                };

                List<int> values = new();

                if (lengthTypeId)
                {
                    // Next 11 are number of subpackets.
                    var mask11Bits = new BitArray(11, true);
                    mask11Bits.Length = bits.Length;

                    var subPacketsBits = new BitArray(bits).And(mask11Bits);
                    subPacketsBits.Length = 11;
                    var subPackets = BitsToInt(subPacketsBits);

                    bits.RightShift(11);
                    length += 11;

                    for (int i = 0; i < subPackets; i++)
                    {
                        var (sumOfVersion, subLength, packetValue) = ProcessPacket(bits);

                        length += subLength;
                        versionSum += sumOfVersion;
                        values.Add(packetValue);
                    }
                }
                else
                {
                    // Next 15 are total length in bits of subpackets.
                    var mask15Bits = new BitArray(15, true);
                    mask15Bits.Length = bits.Length;

                    var totalLengthBits = new BitArray(bits).And(mask15Bits);
                    totalLengthBits.Length = 15;
                    var totalLength = BitsToInt(totalLengthBits);

                    bits.RightShift(15);
                    length += 15;

                    var subLengths = 0;

                    while (subLengths < totalLength)
                    {
                        var (sumOfVersion, subLength, packetValue) = ProcessPacket(bits);

                        subLengths += subLength;
                        versionSum += sumOfVersion;
                        values.Add(packetValue);
                    }

                    length += subLengths;
                }

                value += subPacketsOperation(values);
            }

            return (versionSum, length, value);
        }

        private (int literalValue, int length) ProcessLiteralPacket(BitArray bits)
        {
            int length = 0;

            var mask5Bits = new BitArray(5, true);
            mask5Bits.Length = bits.Length;

            int total = 0;

            while (true)
            {
                length += 5;
                var current = new BitArray(bits).And(mask5Bits);
                var shouldContinue = current[0];

                var digits = current.RightShift(1);

                for (int i = 0; i < 4; i++)
                {
                    total = total << 1;

                    if (digits[i])
                    {
                        total++;
                    }
                }

                bits.RightShift(5);

                // Leading bit is 0, this is the last group.
                if (shouldContinue == false)
                {
                    break;
                }
            }

            return (total, length);
        }

        private BitArray HexToBinary(string input)
        {
            var size = input.Length * 4;
            var bitArray = new BitArray(size);

            bitArray.SetAll(false);

            for (int i = 0; i < input.Length; i++)
            {
                var bit = input[i];
                var segment = HexToBinary(bit);

                segment.Length = size;

                segment.LeftShift(size - ((i + 1) * 4));

                bitArray = bitArray.Xor(segment);
            }

            return bitArray;
        }

        private BitArray HexToBinary(char input)
        {
            byte value = input switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                'A' => 10,
                'B' => 11,
                'C' => 12,
                'D' => 13,
                'E' => 14,
                _ => 15
            };

            return new BitArray(new int[] { value });

        }

        public int BitsToInt(BitArray array)
        {
            int result = 0;

            if (array.Length > 32)
            {
                array = new BitArray(array);
                array.Length = 32;
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == true)
                {
                    result += 1 << array.Length - i - 1;
                }
            }

            return result;
        }

        public static void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }

        public static void PrintValues(IEnumerable myList, int myWidth)
        {
            int i = myWidth;
            foreach (Object obj in myList)
            {
                if (i <= 0)
                {
                    i = myWidth;
                    // Console.WriteLine();
                }
                i--;
                Console.Write("{0}", (bool)obj == true ? "1" : "0");
            }
            Console.WriteLine();
        }
    }
}
                                                                                                                                                                                                                                                                    