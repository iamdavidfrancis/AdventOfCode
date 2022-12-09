using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public record Point2D(int X, int Y) {
        public static Point2D operator +(Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
        public static Point2D operator -(Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
    }

    public static class Point2DExtensions
    {
        public static IEnumerable<Point2D> Neighbors(this Point2D source)
        {
            return new List<Point2D>
            {
                new Point2D(source.X, source.Y + 1),
                new Point2D(source.X, source.Y - 1),
                new Point2D(source.X + 1, source.Y),
                new Point2D(source.X - 1, source.Y),
            };
        }

        public static IEnumerable<Point2D> NeighborsWithDiagonal(this Point2D source)
        {
            return new List<Point2D>
            {
                new Point2D(source.X, source.Y + 1),
                new Point2D(source.X, source.Y - 1),
                new Point2D(source.X + 1, source.Y),
                new Point2D(source.X - 1, source.Y),
                new Point2D(source.X + 1, source.Y + 1),
                new Point2D(source.X - 1, source.Y - 1),
                new Point2D(source.X + 1, source.Y - 1),
                new Point2D(source.X - 1, source.Y + 1),
            };
        }

        public static Point2D Up(this Point2D source, int distance = 1) {
            return source with { Y = source.Y + distance };
        }
        
        public static Point2D Down(this Point2D source, int distance = 1) {
            return source with { Y = source.Y - distance };
        }
        
        public static Point2D Left(this Point2D source, int distance = 1) {
            return source with { X = source.X - distance };
        }
        
        public static Point2D Right(this Point2D source, int distance = 1) {
            return source with { X = source.X + distance };
        }
    }
}
