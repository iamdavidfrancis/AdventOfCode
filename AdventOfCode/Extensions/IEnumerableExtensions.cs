namespace AdventOfCode;

public static class IEnumerableExtensions {
    public static Queue<T> ToQueue<T>(this IEnumerable<T> source) {
        Queue<T> queue = new();

        foreach (var item in source) {
            queue.Enqueue(item);
        }

        return queue;
    }

    public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
    {
        foreach (var item in source)
        {
            action?.Invoke(item);
        }

        return source;
    }
}