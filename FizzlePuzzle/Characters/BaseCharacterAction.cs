using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Item;
using FizzlePuzzle.Scene;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    internal abstract class BaseCharacterAction : FizzleBehaviour
    {
        internal FizzleBox carryingObject;

        internal float Distance { get; private set; } = float.PositiveInfinity;

        internal abstract Ray CameraRay { get; set; }

        protected InteractiveItem CurrentItem { get; set; }

        internal event FizzleEvent<FizzleBox> carriedObject = cube => { };

        internal event FizzleEvent<FizzleBox> releasedObject = cube => { };

        internal event FizzleEvent<FizzleButton> pressedButton = cube => { };

        internal void Carry(FizzleBox box)
        {
            carryingObject = box;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (FizzleScene.TimeCtrl.Rewinding)
            {
                ChangeCurrentItem(null);
            }
            else
            {
                InteractiveItem availableItem = GetAvailableItem();
                if (availableItem)
                {
                    if (!carryingObject)
                    {
                        ChangeCurrentItem(availableItem);
                    }
                    else if (typeof(FizzleBox) != availableItem.GetType())
                    {
                        ChangeCurrentItem(availableItem);
                    }
                    else
                    {
                        ChangeCurrentItem(carryingObject);
                    }
                }
                else if (carryingObject)
                {
                    ChangeCurrentItem(carryingObject);
                }
                else
                {
                    ChangeCurrentItem(null);
                }
            }
        }

        protected InteractiveItem Interact()
        {
            CurrentItem?.Interact(transform);
            if (CurrentItem is FizzleBox && carryingObject)
            {
                carriedObject(carryingObject);
            }
            if (CurrentItem is FizzleBox && !carryingObject)
            {
                releasedObject(carryingObject);
            }
            FizzleButton currentItem = CurrentItem as FizzleButton;
            if (currentItem != null)
            {
                pressedButton(currentItem);
            }
            return CurrentItem;
        }

        protected abstract void ChangeCurrentItem(InteractiveItem item);

        private InteractiveItem GetAvailableItem()
        {
            int notMask1 = FizzleLayerMask.GetNotMask("Player", "Curtain");
            int notMask2 = FizzleLayerMask.GetNotMask("Player");
            Ray cameraRay = CameraRay;
            Vector3 origin = cameraRay.origin;
            cameraRay = CameraRay;
            Vector3 direction = cameraRay.direction;
            RaycastHit raycastHit;
            int layerMask = notMask1;
            double num = Physics.Raycast(origin, direction, out raycastHit, float.PositiveInfinity, layerMask) ? raycastHit.distance : double.PositiveInfinity;
            RaycastHit hitInfo;
            Distance = Physics.Raycast(CameraRay.origin, CameraRay.direction, out hitInfo, float.PositiveInfinity, notMask2) ? hitInfo.distance : float.PositiveInfinity;
            return num > 3.0F ? null : raycastHit.collider.gameObject.GetComponent<InteractiveItem>();
        }
    }
}