namespace AdventOfCode;

public static class AoCMath {
    /// <summary>
    /// Finds the greatest common divisor using the Euclidean Algorithm.
    /// </summary>
    public static ulong GCD(ulong a, ulong b) {
        while (a != 0 && b != 0)
        {
            if (a > b)
            {
                a %= b;
            }
            else 
            {
                b %= a;
            }
        }

        return a | b;
    }

    public static ulong LCM(ulong a, ulong b) {
        return a * (b / GCD(a, b));
    }

    public static ulong LCM(params ulong[] vals) {
        var size = vals.Count();
        ulong acc = 1;

        if (size <= 1)
        {
            throw new InvalidOperationException("Must supply at least two elements");
        }
        else
        {
            acc = vals[0];
            for (int i = 1; i < size; i++)
            {
                acc = LCM(acc, vals[i]);
            }
        }

        return acc;
    }
}