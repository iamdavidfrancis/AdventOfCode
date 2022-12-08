using System.Text.RegularExpressions;

namespace AdventOfCode._2022
{
    internal class Day08 : IAsyncAdventOfCodeProblem
    {
        // Note: For this, I pre-processed the input slightly.
        public async Task RunProblemAsync()
        {
            using (TextReader reader = File.OpenText("./2022/Day08.txt"))
            {
                string? line;

                List<List<int>> trees = new();
                
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    List<int> treeRow = new();

                    treeRow.AddRange(line.Select(c => Int32.Parse(c.ToString())));
                    trees.Add(treeRow);
                }

                Console.WriteLine($"Part 1: {Part1(trees)}");
                Console.WriteLine($"Part 2: {Part2(trees)}");
            }
        }

        private int Part1(List<List<int>> trees)
        {
            int width = trees[0].Count();
            int height = trees.Count;
            List<List<bool>> visibleMap = new();

            for (int i = 0; i < height; i++)
            {
                visibleMap.Add(new List<bool>(new bool[width]));
            }

            // Mark border as visible
            for (int i = 0; i < width; i++) {
                visibleMap.Set(new Point2D(i, 0), true);
                visibleMap.Set(new Point2D(i, height - 1), true);
            }

            for (int i = 0; i < height; i++) {
                visibleMap.Set(new Point2D(0, i), true);
                visibleMap.Set(new Point2D(width - 1, i), true);
            }

            int tallest = 0;
            
            // Check rows
            for (int y = 1; y < height; y++)
            {
                tallest = trees.Get(new Point2D(0, y));

                // From left
                for (int x = 1; x < width; x++)
                {
                    if (trees.Get(new Point2D(x, y)) > tallest)
                    {
                        visibleMap.Set(new Point2D(x, y), true);
                        tallest = trees.Get(new Point2D(x, y));
                    }
                }

                tallest = trees.Get(new Point2D(width - 1, y));

                // From right
                for (int x = width - 2; x > 0; x--)
                {
                    if (trees.Get(new Point2D(x, y)) > tallest)
                    {
                        visibleMap.Set(new Point2D(x, y), true);
                        tallest = trees.Get(new Point2D(x, y));
                    }
                }
            }

            // Check cols
            for (int x = 1; x < width; x++)
            {
                tallest = trees.Get(new Point2D(x, 0));

                // From top
                for (int y = 1; y < height; y++)
                {
                    if (trees.Get(new Point2D(x, y)) > tallest)
                    {
                        visibleMap.Set(new Point2D(x, y), true);
                        tallest = trees.Get(new Point2D(x, y));
                    } 
                }

                tallest = trees.Get(new Point2D(x, height - 1));

                // From bottom
                for (int y = height - 2; y > 0; y--)
                {
                    if (trees.Get(new Point2D(x, y)) > tallest)
                    {
                        visibleMap.Set(new Point2D(x, y), true);
                        tallest = trees.Get(new Point2D(x, y));
                    }
                }
            }

            return visibleMap.SelectMany(t => t.Where(ti => ti == true)).Count();
        }
    
        private int Part2(List<List<int>> trees) {
            int maxScore = 0;

            int width = trees[0].Count;
            int height = trees.Count;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    var newScore = GetTreePart2Score(trees, new Point2D(x,y));
                    if (newScore > maxScore) {
                        maxScore = newScore;
                    }
                }
            }

            return maxScore;
        }

        private int GetTreePart2Score(List<List<int>> trees, Point2D treePos)
        {
            int up = 0;
            int down = 0;
            int left = 0;
            int right = 0;

            int width = trees[0].Count;
            int height = trees.Count;

            int treeHeight = trees.Get(treePos);

            // Check Left
            if (treePos.X > 0) {
                for (int x = treePos.X - 1; x >= 0; x--)
                {
                    left++;
                    if (trees.Get(new Point2D(x, treePos.Y)) >= treeHeight)
                    {
                        break;
                    }
                }
            }

            // Check Right
            if (treePos.X < width - 2) {
                for (int x = treePos.X + 1; x < width; x++)
                {
                    right++;
                    if (trees.Get(new Point2D(x, treePos.Y)) >= treeHeight)
                    {
                        break;
                    }
                }
            }

            // Check Up
            if (treePos.Y > 0) {
                for (int y = treePos.Y - 1; y >= 0; y--)
                {
                    up++;
                    if (trees.Get(new Point2D(treePos.X, y)) >= treeHeight)
                    {
                        break;
                    }
                }
            }

            // Check Down
            if (treePos.Y < height - 1) {
                for (int y = treePos.Y + 1; y < height; y++)
                {
                        down++;
                    if (trees.Get(new Point2D(treePos.X, y)) >= treeHeight)
                    {
                        break;
                    }
                }
            }

            return up*down*left*right;
        }
        
    }
}

