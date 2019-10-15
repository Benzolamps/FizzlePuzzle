using FizzlePuzzle.Utility;

namespace FizzlePuzzle.TimeEffect
{
    internal abstract class ForkController : RewindController, IFork
    {
        protected FizzleStack<IRewindStatus> forkStack;

        protected override void Start()
        {
            base.Start();
            forkStack = new FizzleStack<IRewindStatus>(stackCapacity);
        }

        public void Fork()
        {
            if (forkStack.Count != 0L)
            {
                ForkStatus(forkStack.Pop());
            }
            else
            {
                OnForkStopAction();
            }
        }

        protected virtual void OnForkStopAction()
        {
        }

        protected abstract void ForkStatus(IRewindStatus obj);

        public virtual void BeginForking()
        {
            FizzleStack<IRewindStatus> stack2 = new FizzleStack<IRewindStatus>(stackCapacity);
            reverseStack?.Pour(stack2);
            stack2.Pour(forkStack);
            if (forkStack.Count == 0)
            {
                ForkStatus(PushToStack());
            }
        }

        public virtual void EndForking()
        {
            forkStack?.Clear();
        }

        public virtual void EnableForking()
        {
        }

        public virtual void DisableForking()
        {
            EndForking();
        }
    }
}