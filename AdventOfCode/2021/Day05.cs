using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2021
{
    internal class Day05 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            List<Line> lines = new List<Line>();

            int maxX = 0;
            int maxY = 0;

            using (TextReader reader = File.OpenText("./2021/Day05Input.txt"))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var processedLine = ProcessLine(line);

                    //if (processedLine.Start.X != processedLine.End.X && processedLine.Start.Y != processedLine.End.Y) continue;

                    lines.Add(processedLine);

                    if (processedLine.Start.X > maxX) maxX = processedLine.Start.X;
                    if (processedLine.End.X > maxX) maxX = processedLine.End.X;
                    if (processedLine.Start.Y > maxY) maxY = processedLine.Start.Y;
                    if (processedLine.End.Y > maxY) maxY = processedLine.End.Y;
                }
            }

            int[,] board = new int[maxX+1, maxY+1];

            foreach (var line in lines)
            {
                var startX = line.Start.X < line.End.X ? line.Start.X : line.End.X;
                var endX = line.Start.X < line.End.X ? line.End.X : line.Start.X;

                var startY = line.Start.Y < line.End.Y ? line.Start.Y : line.End.Y;
                var endY = line.Start.Y < line.End.Y ? line.End.Y : line.Start.Y;

                if (startX == endX || startY == endY)
                {
                    foreach (var x in startX..endX)
                    {
                        foreach (var y in startY..endY)
                        {
                            board[x, y]++;
                        }
                    }
                }
                else
                {
                    var spaces = endX-startX;

                    var xDir = line.Start.X < line.End.X ? 1 : -1;
                    var yDir = line.Start.Y < line.End.Y ? 1 : -1;

                    for (int i = 0; i < spaces; i++)
                    {
                        board[line.Start.X + (i*xDir), line.Start.Y + (i*yDir)]++;
                    }
                }
            }

            var dangerousItems = 0;

            foreach (var item in board)
            {
                if (item > 1)
                {
                    dangerousItems++;
                }
            }

            //for (int j = 0; j <= maxY; j++)
            //{
            //    for (int i = 0; i <= maxX; i++)
            //    {
            //        Console.Write(board[i, j] == 0 ? "." : board[i, j]);
            //    }
            //    Console.WriteLine(String.Empty);
            //}

            Console.WriteLine(dangerousItems);
        }

        private Line ProcessLine(string line)
        {
            var matches = Regex.Matches(line, "(\\d+),(\\d+) -> (\\d+),(\\d+)", RegexOptions.Compiled);
            
            if (matches.Count > 0)
            {
                var groups = matches[0].Groups;

                var startX = Convert.ToInt32(groups[1].Value);
                var startY = Convert.ToInt32(groups[2].Value);
                var endX = Convert.ToInt32(groups[3].Value);
                var endY = Convert.ToInt32(groups[4].Value);

                return new Line(new Point2D(startX, startY), new Point2D(endX, endY));
            }

            throw new Exception("Fuck");
        }

        private record Line(Point2D Start, Point2D End);
    }
}
