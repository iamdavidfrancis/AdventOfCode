namespace AdventOfCode.AoC2025;

public class Day04 : IAsyncAdventOfCodeProblem
{
    const string EmptyChar = ".";
    const string PaperChar = "@";

    public async Task RunProblemAsync()
    {
        Matrix<string> grid;
        using (TextReader reader = File.OpenText("./2025/Day04.txt"))
        {
            grid = await reader.ReadFileToMatrix<string>();
        }

        int moveableItems = 0;

        while (true)
        {
            HashSet<Point2D> moveablePoints = [];

            grid.IterateOverEntireMatrix(currentPoint =>
            {
                var neighbors = grid.ValidNeighbors(currentPoint.NeighborsWithDiagonal());

                int occupiedNeighbors = 0;
                
                var item = grid.Get(currentPoint);

                if (item == EmptyChar)
                {
                    return;
                }

                foreach (var neighbor in neighbors)
                {
                    var c = grid.Get(neighbor);
                    if (c != EmptyChar)
                    {
                        occupiedNeighbors++;
                    }
                }

                if (occupiedNeighbors < 4)
                {
                    moveableItems++;
                    moveablePoints.Add(currentPoint);
                }
            });

            if (moveablePoints.Any())
            {
                foreach (var point in moveablePoints)
                {
                    grid.Set(point, EmptyChar);
                }
            }
            else
            {
                break;
            }
        }

        Console.WriteLine(moveableItems);
    }
}
