using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day09 : IAsyncAdventOfCodeProblem
    {
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2022/Day09.txt"))
            {
                string? line;

                List<Point2D> knots1 = Enumerable.Range(1, 2).Select(c => new Point2D(0, 0)).ToList();
                List<Point2D> knots2 = Enumerable.Range(1, 10).Select(c => new Point2D(0, 0)).ToList();
                
                HashSet<Point2D> visitedPoints1 = new HashSet<Point2D>();
                HashSet<Point2D> visitedPoints2 = new HashSet<Point2D>();

                visitedPoints1.Add(new Point2D(0,0));
                visitedPoints2.Add(new Point2D(0,0));
                
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var pieces = line.Split(" ");
                    var direction = pieces[0];
                    var distance = Int32.Parse(pieces[1]);
                    
                    for (int i = 0; i < distance; ++i)
                    {
                        switch (direction) {
                            case "U":
                                knots1[0] = knots1[0].Up();
                                knots2[0] = knots2[0].Up();
                                break;
                            case "D":
                                knots1[0] = knots1[0].Down();
                                knots2[0] = knots2[0].Down();
                                break;
                            case "L":
                                knots1[0] = knots1[0].Left();
                                knots2[0] = knots2[0].Left();
                                break;
                            default:
                                knots1[0] = knots1[0].Right();
                                knots2[0] = knots2[0].Right();
                                break;
                        }

                        for (int j = 0; j < knots1.Count - 1; j++)
                        {
                            ProcessKnotPair(knots1, j, j+1);
                        }

                        for (int j = 0; j < knots2.Count - 1; j++)
                        {
                            ProcessKnotPair(knots2, j, j+1);
                        }

                        
                        if(!visitedPoints1.Contains(knots1.Last()))
                        {
                            visitedPoints1.Add(knots1.Last());
                        }

                        if(!visitedPoints2.Contains(knots2.Last()))
                        {
                            visitedPoints2.Add(knots2.Last());
                        }
                    }
                }

                Console.WriteLine($"Part 1: {visitedPoints1.Count}");
                Console.WriteLine($"Part 2: {visitedPoints2.Count}");
            }
        }

        private void ProcessKnotPair(List<Point2D> knots, int idxA, int idxB)
        {
            var delta = knots[idxA] - knots[idxB];
            if (Math.Abs(delta.X) == 2 && delta.Y == 0)
            {
                knots[idxB] = knots[idxB].Right(delta.X/2);
            }
            else if (Math.Abs(delta.X) == 2 && Math.Abs(delta.Y) == 1)
            {
                knots[idxB] = knots[idxB].Right(delta.X/2);
                knots[idxB] = knots[idxB].Up(delta.Y);
            }
            else if (Math.Abs(delta.X) == 0 && Math.Abs(delta.Y) == 2)
            {
                knots[idxB] = knots[idxB].Up(delta.Y/2);
            }
            else if (Math.Abs(delta.X) == 1 && Math.Abs(delta.Y) == 2)
            {
                knots[idxB] = knots[idxB].Right(delta.X);
                knots[idxB] = knots[idxB].Up(delta.Y/2);
            }
            else if (Math.Abs(delta.X) == 2 && Math.Abs(delta.Y) == 2)
            {
                knots[idxB] = knots[idxB].Right(delta.X/2);
                knots[idxB] = knots[idxB].Up(delta.Y/2);
            }

        }
    }
}

