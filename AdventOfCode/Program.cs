
using AdventOfCode;

var problem = new AdventOfCode.AoC2025.Day06();

if (problem is IAdventOfCodeProblem syncProblem)
{
    syncProblem.RunProblem();
}
else if (problem is IAsyncAdventOfCodeProblem asyncProblem)
{
    await asyncProblem.RunProblemAsync();
}

