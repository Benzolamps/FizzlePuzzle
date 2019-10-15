using FizzlePuzzle.Core;
using FizzlePuzzle.Effect;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal abstract class InteractiveItem : FizzleBehaviour, FizzleItem
    {
        private Outline outline;

        protected override void Awake()
        {
            base.Awake();
            outline = gameObject.AddComponent<Outline>();
            outline.color = 1;
        }

        internal abstract void Interact(Transform player);

        internal void SetAvailable(bool available)
        {
            outline.color = available ? 2 : 1;
        }

        public abstract void Generate(FizzleJson data);
    }
}