using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class ListListExtensions
    {
        public static int Get(this List<List<int>> source, Point2D point)
        {
            return source[point.Y][point.X];
        }

        public static void Set(this List<List<int>> source, Point2D point, int value)
        {
            source[point.Y][point.X] = value;
        }

        public static int Increment(this List<List<int>> source, Point2D point)
        {
            return ++source[point.Y][point.X];
        }

        public static bool Contains(this List<List<int>> source, Point2D point)
        {
            return point.Y >= 0 && source.Count > point.Y && point.X >= 0 && source[point.Y].Count > point.X;
        }

        public static IEnumerable<Point2D> ValidNeighbors(this List<List<int>> source, IEnumerable<Point2D> neighbors)
        {
            return neighbors.Where(n => source.Contains(n));
        }
    }
}
