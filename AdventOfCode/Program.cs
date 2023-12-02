﻿
using AdventOfCode;

var problem = new AdventOfCode._2023.Day02();

if (problem is IAdventOfCodeProblem syncProblem)
{
    syncProblem.RunProblem();
}
else if (problem is IAsyncAdventOfCodeProblem asyncProblem)
{
    await asyncProblem.RunProblemAsync();
}

