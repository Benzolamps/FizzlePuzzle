using System;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    [Serializable]
    internal struct FizzleItemPair
    {
        [SerializeField] internal string m_ItemName;
        [SerializeField] internal ItemWrapper m_ItemPrefab;
    }
}