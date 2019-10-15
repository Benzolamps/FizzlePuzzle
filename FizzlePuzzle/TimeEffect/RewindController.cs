using FizzlePuzzle.Core;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.TimeEffect
{
    internal abstract class RewindController : FizzleBehaviour, IRewind
    {
        protected FizzleStack<IRewindStatus> reverseStack;
        protected FizzleStack<IRewindStatus> rewindStack;
        protected int stackCapacity;

        private static TimeController TimeController => FizzleScene.TimeCtrl;

        protected override void Start()
        {
            base.Start();
            stackCapacity = (int) (1.0F / Time.fixedDeltaTime) * TimeController.MaxSeconds;
            rewindStack = new FizzleStack<IRewindStatus>(stackCapacity);
            reverseStack = new FizzleStack<IRewindStatus>(stackCapacity);
        }

        public virtual void BeginRecording()
        {
        }

        protected abstract IRewindStatus PushToStack();

        public void Record()
        {
            rewindStack.Push(PushToStack());
        }

        public virtual void EndRecording()
        {
            reverseStack?.Clear();
        }

        public virtual void BeginRewinding()
        {
        }

        protected abstract void PopFromStack(IRewindStatus rewindStatus);

        public void Rewind()
        {
            int currentRewindSpeed = TimeController.CurrentRewindSpeed;
            FizzleStack<IRewindStatus> fizzleStack1;
            FizzleStack<IRewindStatus> fizzleStack2;
            if (TimeController.CurrentRewindSpeed < 0)
            {
                fizzleStack1 = rewindStack;
                fizzleStack2 = reverseStack;
            }
            else
            {
                if (TimeController.CurrentRewindSpeed <= 0)
                {
                    return;
                }
                fizzleStack1 = reverseStack;
                fizzleStack2 = rewindStack;
            }

            for (int index = 0; index < Mathf.Abs(currentRewindSpeed) && fizzleStack1.Count != 0L; ++index)
            {
                IRewindStatus rewindStatus = fizzleStack1.Pop();
                PopFromStack(rewindStatus);
                fizzleStack2.Push(rewindStatus);
            }
        }

        public virtual void TimeOutRewinding()
        {
            if (TimeController.CurrentRewindSpeed > 0)
            {
                reverseStack?.Clear();
            }
            else
            {
                if (TimeController.CurrentRewindSpeed >= 0)
                    return;
                rewindStack?.Clear();
            }
        }

        public virtual void PauseRewinding()
        {
        }

        public virtual void EndRewinding()
        {
        }
    }
}