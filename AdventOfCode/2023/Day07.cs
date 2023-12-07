using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace AdventOfCode._2023;

public class Day07 : IAsyncAdventOfCodeProblem
{

    public async Task RunProblemAsync()
    {
        using (TextReader reader = File.OpenText("./2023/Day07.txt"))
        {
            string? line;

            List<Hand> hands = [];
            List<Hand> hands2 = [];
            
            while ((line = await reader.ReadLineAsync()) != null) 
            {
                var parts = line.Split(" ", StringSplitOptions.TrimEntries);
                var hand = Hand.Parse(parts[0]);
                hand.Bid = int.Parse(parts[1]);

                var hand2 = Hand.Parse(parts[0], isPart2: true);
                hand2.Bid = int.Parse(parts[1]);

                hands.Add(hand);
                hands2.Add(hand2);
            }

            hands.Sort();
            hands2.Sort();

            var result1 = hands.Select((h, i) => h.Bid * (i + 1)).Sum();
            var result2 = hands2.Select((h, i) => h.Bid * (i + 1)).Sum();

            Console.WriteLine("Part 1");
            Console.WriteLine(result1);

            Console.WriteLine();

            Console.WriteLine("Part 2");
            Console.WriteLine(result2);
        }
    }

    public class Hand : IComparable<Hand>
    {
        private Hand(int[] values, HandType handType)
        {
            this.Values = values;
            this.HandType = handType;
        }

        public int[] Values { get; private set; }

        public HandType HandType { get; private set; }

        public int Bid { get; set; }

        public int CompareTo(Hand? other)
        {
            if (other == null)
            {
                return 1;
            }

            var value = this.HandType - other.HandType;

            if (value != 0)
            {
                return value;
            }

            for (int i = 0; i < 5; ++i)
            {
                var temp = this.Values[i] - other.Values[i];

                if (temp != 0)
                {
                    return temp;
                }
            }

            return 0;
        }

        public static Hand Parse(string handString, bool isPart2 = false)
        {
            var values = new int[5];

            var chars = handString.ToCharArray();

            for (int i = 0; i < 5; ++i)
            {
                if (char.IsDigit(chars[i]))
                {
                    values[i]  = chars[i] - '0';
                    continue;
                }

                switch (chars[i])
                {
                    case 'T':
                        values[i] = 10;
                        break;
                    case 'J':
                        values[i] = isPart2 ? 1 : 11;
                        break;
                    case 'Q':
                        values[i] = 12;
                        break;
                    case 'K':
                        values[i] = 13;
                        break;
                    default:
                        values[i] = 14;
                        break;
                }
            }

            var groups = values.GroupBy(v => v);

            HandType handType;
            int count;

            if (isPart2 && handString.Contains('J'))
            {
                var noJs = values.Where(v => v != 1).OrderByDescending(v => v);
                var jCount = 5 - noJs.Count();

                var noJGroups = noJs.GroupBy(v => v).OrderByDescending(v => v.Count());

                var biggestGroup = noJGroups.FirstOrDefault();

                switch (noJGroups.Count())
                {
                    case 0:
                        handType = HandType.FiveOfAKind;
                        break;
                    case 1:
                        handType = HandType.FiveOfAKind;
                        break;
                    case 2:
                        var jGroup1Size = noJGroups.First().Count();
                        handType = jCount switch
                        {
                            1 => (jGroup1Size == 2) ? HandType.FullHouse : HandType.FourOfAKind,// Two Pair or Three of a Kind initially
                            2 => HandType.FourOfAKind,// One Pair initially
                            3 => HandType.FourOfAKind,// High Card initially
                            _ => HandType.FiveOfAKind,// High card initially
                        };
                        break;
                    case 3:
                        handType = jCount switch
                        {
                            1 => HandType.ThreeOfAKind,// One Pair initially 
                            _ => HandType.ThreeOfAKind,// High Card initially
                        };
                        break;
                    case 4:
                    default: 
                        handType = HandType.OnePair;
                        break;
                }
            }
            else
            {
                switch (groups.Count())
                {
                    case 1:
                        handType = HandType.FiveOfAKind;
                        break;
                    case 2:
                        handType = (groups.Max(c => c.Count()) == 3) ? HandType.FullHouse : HandType.FourOfAKind;
                        break;
                    case 3:
                        count = groups.First().Count();
                        handType = (groups.Max(c => c.Count()) == 3) ? HandType.ThreeOfAKind : HandType.TwoPair;
                        break;
                    case 4:
                        handType = HandType.OnePair;
                        break;
                    default:
                        handType = HandType.HighCard;
                        break;
                }
            }

            return new Hand(values, handType);
        }
    }

    public enum HandType
        {
            HighCard = 0,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }
}

