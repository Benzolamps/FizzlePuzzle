using FizzlePuzzle.Item;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    internal class ForkCharacterAction : BaseCharacterAction
    {
        internal InteractiveItem currentInteractiveItem;

        internal override Ray CameraRay { get; set; }

        internal bool IsCarrying { get; set; }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!currentInteractiveItem || !(currentInteractiveItem == CurrentItem))
            {
                return;
            }
            if (currentInteractiveItem.GetType() == typeof(FizzleBox))
            {
                if (IsCarrying == carryingObject)
                {
                    return;
                }
                Interact();
            }
            else
            {
                Interact();
            }
        }

        protected override void ChangeCurrentItem(InteractiveItem item)
        {
            CurrentItem = item;
        }

        internal void ReleaseAll()
        {
            carryingObject?.Release(transform);
        }
    }
}
