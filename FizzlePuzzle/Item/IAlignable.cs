using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal interface IAlignable
    {
        float AlignHeight { get; }

        Transform AlignTransform { get; }
    }
}
