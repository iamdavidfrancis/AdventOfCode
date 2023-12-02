using System.Text.Json;

namespace AdventOfCode._2022
{
    // Part 1
    public class Day14 : IAsyncAdventOfCodeProblem
    {
        private const bool Part2 = true;

        public async Task RunProblemAsync()
        {
            List<List<Point2D>> structures = new();

            using (TextReader reader = File.OpenText("./2022/Day14.txt"))
            {
                string? line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var points = line.Split(" -> ")
                        .Select(c => c.Split(",")
                            .Select(c1 => Int32.Parse(c1))
                            .ToList())
                        .Select(cs => new Point2D(cs[0], cs[1]))
                        .ToList();

                    structures.Add(points);
                }
            }

            // Trial and error got to this point. Who needs smart when you have *RAM* (It wasn't that much RAM).
            int minX = Part2 ? 300 : 500;
            int minY = 0;
            int maxX = Part2 ? 700 : 500;
            int maxY = 0;

            Point2D source = new Point2D(500, 0);

            Dictionary<Point2D, CaveItem> map = new();

            foreach(var structure in structures) 
            {
                Point2D next = structure[0];
                for(int i = 1; i < structure.Count; i++)
                {
                    Point2D current = next;
                    next = structure[i];

                    // Find bounds
                    if (!Part2)
                    {
#pragma warning disable CS0162 // Unreachable code detected
                        if (current.X < minX)
                        {
                            minX = current.X;
                        }
                        if (current.X > maxX)
                        {
                            maxX = current.X;
                        }

                        if (next.X < minX)
                        {
                            minX = next.X;
                        }
                        if (next.X > maxX)
                        {
                            maxX = next.X;
                        }
#pragma warning restore CS0162 // Unreachable code detected
                    }

                    if (current.Y < minY)
                    {
                        minY = current.Y;
                    }
                    if (current.Y > maxY)
                    {
                        maxY = current.Y;
                    }  

                    if (next.Y < minY)
                    {
                        minY = next.Y;
                    }
                    if (next.Y > maxY)
                    {
                        maxY = next.Y;
                    }                

                    // Fill rocks
                    if (current.X == next.X)
                    {
                        // Vertical
                        Point2D start;
                        Point2D end;
                        if (current.Y < next.Y)
                        {
                            start = current;
                            end = next;
                        }
                        else
                        {
                            end = current;
                            start = next;
                        }

                        var yVals = Enumerable.Range(start.Y, end.Y - start.Y + 1);
                        foreach (var y in yVals)
                        {
                            map.TryAdd(start with { Y = y}, CaveItem.Rock);
                        }
                    }
                    else
                    {
                        // Horizontal
                        Point2D start;
                        Point2D end;
                        if (current.X < next.X)
                        {
                            start = current;
                            end = next;
                        }
                        else
                        {
                            end = current;
                            start = next;
                        }

                        var xVals = Enumerable.Range(start.X, end.X - start.X + 1);
                        foreach (var x in xVals)
                        {
                            map.TryAdd(start with { X = x}, CaveItem.Rock);
                        }
                    }
                }
            }
            
            // Fill in map.
            if (Part2)
            {
                maxY += 2;
            }

            for (int j = minY; j <= maxY; j++)
            {
                for (int i = minX - 1; i <= maxX + 1; i++)
                {
                    if (j == 0 && i == 500)
                    {
                        map.Add(source, CaveItem.Source);
                    }
                    else if (!map.ContainsKey(new Point2D(i, j)))
                    {
                        map.Add(new Point2D(i, j), CaveItem.Air);
                    }
                }
            }

            if (Part2)
            {
                for (int i = minX - 1; i <= maxX + 1; i++)
                {
                    map[new Point2D(i, maxY)] = CaveItem.Rock;
                }
            }

            int count = 0;
            while (!AddSand(map, minX, maxX, maxY))
            {
                count++;
            }
            
            PrintMap(map);

            Console.WriteLine(count + (Part2 ? 1 : 0));

        }

        private void PrintMap(Dictionary<Point2D, CaveItem> map)
        {
            Console.Clear();
            var points = map.Keys.OrderBy(k => k.Y).ThenBy(k => k.X);

            var currY = 0;
            foreach (var point in points)
            {
                if (point.Y != currY)
                {
                    Console.WriteLine();
                    currY = point.Y;
                }

                var item = map[point];

                switch (item) {
                    case CaveItem.Rock:
                        Console.Write("#");
                        break;
                    case CaveItem.Air:
                        Console.Write(" "); // Should be a dot, but I liked it better this way.
                        break;
                    case CaveItem.Source:
                        Console.Write("+");
                        break;
                    case CaveItem.Sand:
                        Console.Write("o");
                        break;
                }
            }
            
            Console.WriteLine();
            Console.WriteLine();
        }

        // returns true when done
        private bool AddSand(Dictionary<Point2D, CaveItem> map, int minX, int maxX, int maxY)
        {
            Point2D pos = new Point2D(500, 0);

            var isDone = false;

            while (!isDone)
            {
                var (nextPos, isLanded, isVoid) = GetNextPos(map, pos, minX, maxX, maxY);

                if (isLanded)
                {
                    map[nextPos] = CaveItem.Sand;
                    isDone = true;

                    if (nextPos == new Point2D(500, 0))
                    {
                        return true;
                    }
                }
                else if (isVoid)
                {
                    return true;
                }
                pos = nextPos;
            }
            

            // Didn't hit the void
            return false;
        }

        private (Point2D nextPos, bool isLanded, bool isVoid) GetNextPos(Dictionary<Point2D, CaveItem> map, Point2D pos, int minX, int maxX, int maxY)
        {
            // Naming sucks, I get it
            var down = pos.Up();
            var downLeft = pos.Up().Left();
            var downRight = pos.Up().Right();

            if (pos.X < minX || pos.X > maxX || pos.Y >= maxY)
            {
                return (pos, false, true);
            }

            if (map.TryGetValue(down, out var downItem) && downItem == CaveItem.Air)
            {
                return (down, false, false);
            }
            else if (map.TryGetValue(downLeft, out var downLeftItem) && downLeftItem == CaveItem.Air)
            {
                return (downLeft, false, false);
            }
            else if (map.TryGetValue(downRight, out var downRightItem) && downRightItem == CaveItem.Air)
            {
                return (downRight, false, false);
            }
            else
            {
                // No open spots below
                return (pos, true, false);
            }
        }

        private enum CaveItem {
            Rock,
            Air,
            Source,
            Sand
        }
    }
}

