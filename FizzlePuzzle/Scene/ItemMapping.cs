using FizzlePuzzle.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    [Serializable]
    internal struct ItemMapping
    {
        [SerializeField]
        private List<FizzleItemPair> m_FizzleItemPairs;

        internal ItemWrapper GetFizzleItem(string itemName)
        {
            try
            {
                return m_FizzleItemPairs.Single(item => item.m_ItemName == itemName).m_ItemPrefab;
            }
            catch (InvalidOperationException)
            {
                throw new FizzleException("Can't find item class: " + itemName);
            }
        }
    }
}

