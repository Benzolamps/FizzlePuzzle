using System.Collections;
using FizzlePuzzle.Item;
using FizzlePuzzle.Scene;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    internal class FirstPersonCharacterAction : BaseCharacterAction
    {
        private bool canUse = true;

        internal InteractiveItem currentInteractiveItem;

        internal override Ray CameraRay
        {
            get
            {
                return FizzleScene.Camera.ScreenPointToRay(Input.mousePosition);
            }
            set
            {
            }
        }

        protected override void ChangeCurrentItem(InteractiveItem item)
        {
            CurrentItem?.SetAvailable(false);
            (CurrentItem = item)?.SetAvailable(true);
        }

        protected override void Update()
        {
            base.Update();
            if (!canUse)
            {
                return;
            }

            StartCoroutine(Input.GetButtonDown("Use") ? InternalInteract() : JustWait());
        }

        private IEnumerator InternalInteract()
        {
            FirstPersonCharacterAction personCharacterAction = this;
            personCharacterAction.canUse = false;
            yield return new WaitForFixedUpdate();
            personCharacterAction.canUse = true;
            personCharacterAction.currentInteractiveItem = personCharacterAction.Interact();
            yield return new WaitForFixedUpdate();
            personCharacterAction.currentInteractiveItem = null;
        }

        private IEnumerator JustWait()
        {
            yield return new WaitForFixedUpdate();
            currentInteractiveItem = null;
        }
    }
}
