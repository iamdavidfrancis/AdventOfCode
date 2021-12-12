
using AdventOfCode;

var problem = new AdventOfCode._2015.Day01();

if (problem is IAdventOfCodeProblem syncProblem)
{
    syncProblem.RunProblem();
}
else if (problem is IAsyncAdventOfCodeProblem asyncProblem)
{
    await asyncProblem.RunProblemAsync();
}

