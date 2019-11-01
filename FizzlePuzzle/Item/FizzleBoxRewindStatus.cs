using FizzlePuzzle.TimeEffect;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal struct FizzleBoxRewindStatus : IRewindStatus
    {
        internal Vector3 position;
        internal Quaternion rotation;
        internal Vector3 velocity;
        internal Vector3 angularVelocity;
        internal Transform carrier;
    }
}
