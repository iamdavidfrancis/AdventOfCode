using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class ListListExtensions
    {
        public static T Get<T>(this List<List<T>> source, Point2D point)
        {
            return source[point.Y][point.X];
        }

        public static void Set<T>(this List<List<T>> source, Point2D point, T value)
        {
            source[point.Y][point.X] = value;
        }

        public static int Increment(this List<List<int>> source, Point2D point)
        {
            return ++source[point.Y][point.X];
        }

        public static bool Contains<T>(this List<List<T>> source, Point2D point)
        {
            return point.Y >= 0 && source.Count > point.Y && point.X >= 0 && source[point.Y].Count > point.X;
        }

        public static IEnumerable<Point2D> ValidNeighbors<T>(this List<List<T>> source, IEnumerable<Point2D> neighbors)
        {
            return neighbors.Where(n => source.Contains(n));
        }
    }
}
