namespace AdventOfCode;

public static class IEnumerableExtensions {
    public static Queue<T> ToQueue<T>(this IEnumerable<T> source) {
        Queue<T> queue = new();

        foreach (var item in source) {
            queue.Enqueue(item);
        }

        return queue;
    }
}