namespace AdventOfCode
{
    internal interface IAdventOfCodeProblem
    {
        void RunProblem();
    }

    internal interface IAsyncAdventOfCodeProblem
    {
        Task RunProblemAsync();
    }
}