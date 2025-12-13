namespace AdventOfCode.Helpers;

internal class Matrix<T> : List<List<T>>, IEquatable<Matrix<T>>
{
    public int Width => this.FirstOrDefault()?.Count ?? 0;
    public int Height => this.Count;

    public bool Equals(Matrix<T>? other)
    {
        if (other is null) return false;

        return this == other;
    }

    public static bool operator ==(Matrix<T> a, Matrix<T> b)
    {
        if (a is null)
        {
            return b is null;
        }

        if (b is null)
        {
            return false;
        }

        if (a.Count != b.Count)
        { 
            return false;
        }
       
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i].Count != b[i].Count)
            {
                return false;
            }
        }

        var isEqual = true;
        a.IterateOverEntireMatrix(p =>
        {
            if (!EqualityComparer<T>.Default.Equals(a.Get(p), b.Get(p)))
            {
                isEqual = false;
            }
        });

        return isEqual;
    }

    public static bool operator !=(Matrix<T> a, Matrix<T> b)
    {
        return !(a == b);
    }
}
