using System.Collections.Generic;
using System.Linq;
using FizzlePuzzle.Core;
using FizzlePuzzle.Item;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    internal class MapGenerate
    {
        private string config;
        private readonly string objects;
        private ItemMapping itemMapping;
        
        private static readonly List<string> orders = new List<string>
        {
            "FizzleButton", "FizzleLogicCurtain", "PressurePlate", "FizzleTrigger", "FizzleElevator", "FizzleBarrier", "FizzleBox", "FizzleBoxRewind", "FizzleScript"
        };

        internal MapGenerate(ItemMapping itemMapping, string config, string objects)
        {
            this.itemMapping = itemMapping;
            this.config = config;
            this.objects = objects;
        }
        
        internal void Generate(Transform itemContainer, Transform terrainContainer)
        {
            FizzleJson objects = new FizzleJson(this.objects);
            List<FizzleJson> list = objects.ToList();
            list.Sort((a, b) => orders.IndexOf(a["class"].ToString()) - orders.IndexOf(b["class"].ToString()));
            foreach (FizzleJson data in objects)
            {
                ItemWrapper fizzleItem = itemMapping.GetFizzleItem(data["class"].ToString());
                Transform parent = fizzleItem.WrappedItem is FizzleCube ? terrainContainer : itemContainer;
                ItemWrapper itemWrapper = FizzleBehaviour.Spawn(fizzleItem, parent, data.GetOrDefault("name", "New " + fizzleItem.WrappedItem.GetType().Name));
                FizzleJson fizzleJson = data["position"];
                itemWrapper.transform.transform.position = new Vector3(fizzleJson[0].ToObject<float>(), fizzleJson[1].ToObject<float>(), fizzleJson[2].ToObject<float>());
                itemWrapper.Generate(data);
            }
        }
    }
}
