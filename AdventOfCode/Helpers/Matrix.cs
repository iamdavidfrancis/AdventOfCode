namespace AdventOfCode.Helpers;

internal class Matrix<T> : List<List<T>>
{
    public int Width => this.FirstOrDefault()?.Count ?? 0;
    public int Height => this.Count;
}
