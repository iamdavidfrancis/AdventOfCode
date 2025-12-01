namespace AdventOfCode.Helpers;

public class Dial
{
    public Dial(int position, int min = 0, int max = 99)
    {
        Position = position;
        Min = min;
        Max = max;
    }

    public int Position { get; internal set;}
    public int Min { get; }
    public int Max { get; }

    public int Rotate(string instruction)
    {
        var direction = instruction[0];
        var steps = int.Parse(instruction[1..]);
        return Rotate(direction.ToString(), steps);
    }

    public int Rotate(string direction, int steps = 1)
    {
        // Console.Write($"The dial is rotated {direction}{steps} to point at ");
        var initialPosition = Position;
        var crossedZero = steps / (Max - Min + 1);

        steps %= (Max - Min + 1);

        if (direction == "L")
        {
            Position -= steps;
            if (Position < Min)
            {
                Position = Max - (Min - Position - 1);

                if (initialPosition != Min)
                {
                    crossedZero++;
                }
            }
        }
        else if (direction == "R")
        {
            Position += steps;
            if (Position > Max)
            {
                Position = Min + (Position - Max - 1);
                crossedZero++;

                if (Position == Min)
                {
                    crossedZero--;
                }
            }
        }

        // Console.WriteLine($"{Position}.{(crossedZero > 0 ? $" Crossed zero {crossedZero} time(s)." : "")}");
        return crossedZero;
    }
}