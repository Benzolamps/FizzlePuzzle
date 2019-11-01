using System.Diagnostics.CodeAnalysis;

namespace FizzlePuzzle.TimeEffect
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    internal interface IRewind
    {
        void BeginRecording();

        void Record();

        void EndRecording();

        void BeginRewinding();

        void Rewind();

        void PauseRewinding();

        void TimeOutRewinding();

        void EndRewinding();
    }
}

