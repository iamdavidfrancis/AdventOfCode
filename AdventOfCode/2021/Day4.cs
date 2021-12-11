using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2021
{
    internal class Day4 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2021/Day4Input.txt"))
            {
                var bingoNumberInput = await reader.ReadLineAsync();

                if (bingoNumberInput == null)
                {
                    return;
                }

                var bingoArray = bingoNumberInput.Split(',').Select(x => Convert.ToInt32(x));

                var boards = await GetBingoBoardsAsync(reader);

                int wonBoardCount = 0;
                int totalBoards = boards.Count;

                int lastWinnerScore = 0;

                foreach (var number in bingoArray)
                {
                    MarkBoards(boards, number);
                    var (isBingo, winningBoards, boardIndexes) = ScanBoards(boards);

                    if (isBingo)
                    {
                        var bingoCount = winningBoards.Count;
                        wonBoardCount += bingoCount;

                        for (int i = bingoCount - 1; i >= 0; i--)
                        {
                            var score = ScoreBoard(winningBoards![i]);
                            lastWinnerScore = score * number;

                            if (wonBoardCount == totalBoards)
                            {
                                break;
                            }
                            else
                            {
                                boards.RemoveAt(boardIndexes[i]);
                            }
                        }

                        if (wonBoardCount == totalBoards)
                        {
                            break;
                        }
                    }
                }

                Console.WriteLine(lastWinnerScore);
            }
        }

        private async Task<List<Tuple<int, bool>[,]>> GetBingoBoardsAsync(TextReader reader)
        {
            List<Tuple<int, bool>[,]> boards = new();

            while ((await reader.ReadLineAsync()) != null)
            {
                var board = new Tuple<int, bool>[5, 5];

                for (int i = 0; i < 5; i++)
                {
                    var line = await reader.ReadLineAsync();

                    if (line == null)
                    {
                        throw new Exception("Incorrect number of lines.");
                    }

                    var numbers = line.Split(' ').Where(x => x != "").ToList();

                    for (int j = 0; j < 5; j++)
                    {
                        board[i, j] = new Tuple<int, bool>(Convert.ToInt32(numbers[j]), false);
                    }
                }

                boards.Add(board);
            }

            return boards;
        }

        private void MarkBoards(List<Tuple<int, bool>[,]> boards, int number)
        {
            foreach (var board in boards)
            {
                MarkBoard(board, number);
            }
        }

        private void MarkBoard(Tuple<int, bool>[,] board, int number)
        {
            for (int i = 0; i < 5; i++)
            {
                // Scan each row then each column
                for (int j = 0; j < 5; j++)
                {
                    if (board[i, j].Item1 == number)
                    {
                        board[i, j] = new Tuple<int, bool>(number, true);
                    }

                    if (board[j, i].Item1 == number)
                    {
                        board[j, i] = new Tuple<int, bool>(number, true);
                    }
                }
            }
        }

        private (bool isBingo, List<Tuple<int, bool>[,]> board, List<int> boardIdx) ScanBoards(List<Tuple<int, bool>[,]> boards)
        {
            List<Tuple<int, bool>[,]> solvedBoards = new();
            List<int> boardIndexes = new();

            int idx = 0;
            foreach (var board in boards)
            {
                var isBingo = ScanBoard(board);

                if (isBingo)
                {
                    solvedBoards.Add(board);
                    boardIndexes.Add(idx);
                }

                idx++;
            }

            return (solvedBoards.Count > 0, solvedBoards, boardIndexes);
        }

        private bool ScanBoard(Tuple<int, bool>[,] board)
        {
            bool isBingo = false;

            for (int i = 0; i < 5; i++)
            {
                var rowHits = 0;
                var colHits = 0;

                // Scan each row then each column
                for (int j = 0; j < 5; j++)
                {
                    if (board[i, j].Item2 == true)
                    {
                        rowHits++;
                    }

                    if (board[j, i].Item2 == true)
                    {
                        colHits++;
                    }
                }

                if (rowHits == 5 || colHits == 5)
                {
                    isBingo = true;
                    break;
                }
            }

            return isBingo;
        }

        private int ScoreBoard(Tuple<int, bool>[,] board)
        {
            var sum = 0;

            foreach (var item in board)
            {
                if (item.Item2 == false)
                {
                    sum += item.Item1;
                }
            }

            return sum;
        }
    }
}
