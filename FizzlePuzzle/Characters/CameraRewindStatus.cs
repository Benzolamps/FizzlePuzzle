using FizzlePuzzle.TimeEffect;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    internal struct CameraRewindStatus : IRewindStatus
    {
        internal Vector3 position;
        internal Quaternion rotation;
    }
}
