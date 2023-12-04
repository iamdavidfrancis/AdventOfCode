namespace AdventOfCode;

internal static class MatrixExtensions
{
    public static async Task<Matrix<T>> ReadFileToMatrix<T>(this TextReader reader, T? rightPad = default)
    {
        Matrix<T> matrix = [];

        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            var row = line.Select(c => c.ChangeType<T>()).ToList();
            if (rightPad != null)
            {
                row.Add(rightPad);
            }
            matrix.Add(row);
        }

        return matrix;
    }
}