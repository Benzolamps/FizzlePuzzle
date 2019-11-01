using FizzlePuzzle.TimeEffect;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal struct FizzleElevatorRewindStatus : IRewindStatus
    {
        internal Vector3 position;
        internal FizzleElevatorStatus status;
    }
}
