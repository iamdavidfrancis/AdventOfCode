namespace AdventOfCode.Helpers;

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
            source.Up(),
            source.Down(),
            source.Right(),
            source.Left(),
        };
    }

    public static IEnumerable<Point2D> NeighborsWithDiagonal(this Point2D source)
    {
        return new List<Point2D>
        {
            source.Up(),
            source.Down(),
            source.Right(),
            source.Left(),
            source.Up().Right(),
            source.Up().Left(),
            source.Down().Right(),
            source.Down().Left(),
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
