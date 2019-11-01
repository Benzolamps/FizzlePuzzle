using System.Diagnostics.CodeAnalysis;

namespace FizzlePuzzle.TimeEffect
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    internal interface IFork
    {
        void BeginForking();

        void Fork();

        void EndForking();

        void EnableForking();

        void DisableForking();
    }
}

