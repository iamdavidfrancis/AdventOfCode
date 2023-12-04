namespace AdventOfCode;

internal static class MatrixExtensions
{
    public static async Task<Matrix<T>> ReadFileToMatrix<T>(this TextReader reader)
    {
        Matrix<T> matrix = [];

        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            var row = line.Select(c => c.ChangeType<T>()).ToList();
            matrix.Add(row);
        }

        return matrix;
    }
}