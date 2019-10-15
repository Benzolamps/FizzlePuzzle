using FizzlePuzzle.Item;
using FizzlePuzzle.TimeEffect;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    internal struct FirstPersonRewindStatus : IRewindStatus
    {
        internal Vector3 position;
        internal Quaternion rotation;
        internal Vector3 animationVelocity;
        internal bool grounded;
        internal Ray cameraRay;
        internal InteractiveItem currentItem;
        internal bool isCarrying;
    }
}
