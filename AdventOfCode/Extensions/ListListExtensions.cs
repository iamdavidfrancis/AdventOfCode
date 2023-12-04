using System.Drawing;

namespace AdventOfCode;

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

    public static Point2D FindPoint<T>(this List<List<T>> source, T searchVal)
    {
        int yMax = source.Count;
        int xMax = source[0].Count;

        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                var searchCoords = new Point2D(x, y);
                if (source.Get(searchCoords)!.Equals(searchVal))
                {
                    return searchCoords;
                }
            }
        }

        throw new InvalidOperationException("Could not find value in source");
    }

    public static List<Point2D> FindPoints<T>(this List<List<T>> source, T searchVal) => source.FindPoints((point) => source.Get(point)!.Equals(searchVal));

    public static List<Point2D> FindPoints<T>(this List<List<T>> source, Func<Point2D, bool> searchAction)
    {
        List<Point2D> results = new();

        source.IterateOverEntireMatrix(point => 
        {
            if (searchAction.Invoke(point))
            {
                results.Add(point);
            }
        });

        return results;
    }

    public static void IterateOverEntireMatrix<T>(this List<List<T>> source, Action<Point2D> action)
    {
        int yMax = source.Count;
        int xMax = source[0].Count;

        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                var searchCoords = new Point2D(x, y);
                action.Invoke(searchCoords);
            }
        }
    }

    public static List<List<TOut>> Map<TIn, TOut>(this List<List<TIn>> source, Func<TIn, TOut> mapFunc)
    {
        var result = new List<List<TOut>>();

        foreach (var row in source) {
            var resultRow = new List<TOut>();

            foreach (var item in row) {
                resultRow.Add(mapFunc(item));
            }

            result.Add(resultRow);
        }
    
        return result;
    }
}
