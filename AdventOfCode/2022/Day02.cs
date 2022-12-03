namespace AdventOfCode._2022
{
    internal class Day02 : IAsyncAdventOfCodeProblem
    {
        private static readonly Dictionary<string, GameEnum> OpponentMap = new Dictionary<string, GameEnum>
        {
            { "A", GameEnum.Rock },
            { "B", GameEnum.Paper },
            { "C", GameEnum.Scissors }
        };

        private static readonly Dictionary<string, GameOutcome> PlayerMap = new Dictionary<string, GameOutcome>
        {
            { "X", GameOutcome.Lose },
            { "Y", GameOutcome.Draw },
            { "Z", GameOutcome.Win }
        };

        private static readonly Dictionary<GameEnum, int> PointValue = new Dictionary<GameEnum, int>
        {
            { GameEnum.Rock, 1 },
            { GameEnum.Paper, 2 },
            { GameEnum.Scissors, 3 }
        };

        private enum GameEnum {
            Rock,
            Paper,
            Scissors,
        }

        private enum GameOutcome {
            Lose,
            Draw,
            Win,
        }

        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2022/Day02.txt"))
            {
                string? line;

                int playerScore = 0;

                while ((line = await reader.ReadLineAsync()) != null) {
                    var splitLine = line.Split(" ");
                    var opponent = OpponentMap[splitLine[0]];
                    var playerOutcome = PlayerMap[splitLine[1]];

                    GameEnum player;
                    switch (playerOutcome) {
                        case GameOutcome.Draw:
                            player = opponent;
                            break;
                        case GameOutcome.Win:
                            player = WinningChoice(opponent);
                            break;
                        default:
                            player = LosingChoice(opponent);
                            break;
                    }

                    playerScore += CalculateWin(player, opponent) + PointValue[player];
                }

                Console.WriteLine(playerScore);
            }
        }

        private GameEnum LosingChoice(GameEnum opponent) {
            switch (opponent) {
                case GameEnum.Rock:
                    return GameEnum.Scissors;
                case GameEnum.Paper:
                    return GameEnum.Rock;
                default:
                    return GameEnum.Paper;
            }
        }

        private GameEnum WinningChoice(GameEnum opponent) {
            switch (opponent) {
                case GameEnum.Rock:
                    return GameEnum.Paper;
                case GameEnum.Paper:
                    return GameEnum.Scissors;
                default:
                    return GameEnum.Rock;
            }
        }

        private int CalculateWin(GameEnum player, GameEnum opponent) {
            if (player == opponent) {
                return 3;
            }

            switch (player) {
                case GameEnum.Rock:
                    return (opponent == GameEnum.Scissors) ? 6 : 0;
                case GameEnum.Paper:
                    return (opponent == GameEnum.Rock) ? 6 : 0;
                default:
                    return (opponent == GameEnum.Paper) ? 6 : 0;
            }
        }
    }
}