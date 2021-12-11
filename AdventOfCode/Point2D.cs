using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public record Point2D(int X, int Y);

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
    }
}
