using System;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    [Serializable]
    internal struct SceneInfo
    {
        [SerializeField] internal TextAsset m_ObjectsText;
        [SerializeField] internal TextAsset m_ConfigText;
    }
}
