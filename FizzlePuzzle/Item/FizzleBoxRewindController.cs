using FizzlePuzzle.Characters;
using FizzlePuzzle.Scene;
using FizzlePuzzle.TimeEffect;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzleBoxRewindController : RewindController
    {
        private Rigidbody rigidbody;
        private FizzleBox fizzleBox;

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
            fizzleBox = GetComponent<FizzleBox>();
        }

        protected override IRewindStatus PushToStack()
        {
            return new FizzleBoxRewindStatus
            {
                position = transform.position,
                rotation = transform.rotation,
                velocity = rigidbody.velocity,
                angularVelocity = rigidbody.angularVelocity,
                carrier = fizzleBox.Carrier == FizzleScene.FirstPersonCtrl.transform ? fizzleBox.Carrier : null
            };
        }

        protected override void PopFromStack(IRewindStatus obj)
        {
            FizzleBoxRewindStatus status = (FizzleBoxRewindStatus) obj;
            transform.position = status.position;
            transform.rotation = status.rotation;
            rigidbody.velocity = status.velocity;
            rigidbody.angularVelocity = status.angularVelocity;
            if (fizzleBox.Carrier && !status.carrier)
            {
                fizzleBox.Release(fizzleBox.Carrier);
            }
            else
            {
                if (fizzleBox.Carrier || !status.carrier)
                {
                    return;
                }

                status.carrier.GetComponent<BaseCharacterAction>().carryingObject?.Release(status.carrier);
                fizzleBox.Carry(status.carrier);
            }
        }
    }
}
