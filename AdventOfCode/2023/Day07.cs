namespace AdventOfCode._2023;

public class Day07 : AdventBase
{
    public override string FilePath => "./2023/Day07.txt";

    public override async Task<AdventResult> RunInternal(TextReader reader)
    {
            string? line;

            List<Day07Hand> hands = [];
            List<Day07Hand> hands2 = [];
            
            while ((line = await reader.ReadLineAsync()) != null) 
            {
                var parts = line.Split(" ", StringSplitOptions.TrimEntries);
                var hand = Day07Hand.Parse(parts[0]);
                hand.Bid = int.Parse(parts[1]);

                var hand2 = Day07Hand.Parse(parts[0], isPart2: true);
                hand2.Bid = int.Parse(parts[1]);

                hands.Add(hand);
                hands2.Add(hand2);
            }

            hands.Sort();
            hands2.Sort();

            var result1 = hands.Select((h, i) => h.Bid * (i + 1)).Sum();
            var result2 = hands2.Select((h, i) => h.Bid * (i + 1)).Sum();

            return new(result1, result2);
    }

    public class Day07Hand : IComparable<Day07Hand>
    {
        private Day07Hand(int[] values, Day07HandType handType)
        {
            this.Values = values;
            this.HandType = handType;
        }

        public int[] Values { get; private set; }

        public Day07HandType HandType { get; private set; }

        public int Bid { get; set; }

        public int CompareTo(Day07Hand? other)
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

        public static Day07Hand Parse(string handString, bool isPart2 = false)
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

            Day07HandType handType;
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
                        handType = Day07HandType.FiveOfAKind;
                        break;
                    case 1:
                        handType = Day07HandType.FiveOfAKind;
                        break;
                    case 2:
                        var jGroup1Size = noJGroups.First().Count();
                        handType = jCount switch
                        {
                            1 => (jGroup1Size == 2) ? Day07HandType.FullHouse : Day07HandType.FourOfAKind,// Two Pair or Three of a Kind initially
                            2 => Day07HandType.FourOfAKind,// One Pair initially
                            3 => Day07HandType.FourOfAKind,// High Card initially
                            _ => Day07HandType.FiveOfAKind,// High card initially
                        };
                        break;
                    case 3:
                        handType = jCount switch
                        {
                            1 => Day07HandType.ThreeOfAKind,// One Pair initially 
                            _ => Day07HandType.ThreeOfAKind,// High Card initially
                        };
                        break;
                    case 4:
                    default: 
                        handType = Day07HandType.OnePair;
                        break;
                }
            }
            else
            {
                switch (groups.Count())
                {
                    case 1:
                        handType = Day07HandType.FiveOfAKind;
                        break;
                    case 2:
                        handType = (groups.Max(c => c.Count()) == 3) ? Day07HandType.FullHouse : Day07HandType.FourOfAKind;
                        break;
                    case 3:
                        count = groups.First().Count();
                        handType = (groups.Max(c => c.Count()) == 3) ? Day07HandType.ThreeOfAKind : Day07HandType.TwoPair;
                        break;
                    case 4:
                        handType = Day07HandType.OnePair;
                        break;
                    default:
                        handType = Day07HandType.HighCard;
                        break;
                }
            }

            return new Day07Hand(values, handType);
        }
    }

    public enum Day07HandType
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

