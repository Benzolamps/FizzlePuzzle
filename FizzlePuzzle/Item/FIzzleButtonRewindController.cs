using FizzlePuzzle.TimeEffect;

namespace FizzlePuzzle.Item
{
    internal class FizzleButtonRewindController : RewindController
    {
        private FizzleButton button;
        private bool lastActivate;

        protected override void Awake()
        {
            base.Awake();
            button = GetComponent<FizzleButton>();
        }

        public override void BeginRewinding()
        {
            base.BeginRewinding();
            lastActivate = button.Activated;
        }

        protected override IRewindStatus PushToStack()
        {
            return new FizzleButtonRewindStatus
            {
                activated = button.Activated
            };
        }

        protected override void PopFromStack(IRewindStatus rewindStatus)
        {
            if (!(((FizzleButtonRewindStatus) rewindStatus).activated ^ lastActivate))
                return;
            button.ToggleActive();
            lastActivate = !lastActivate;
        }
    }
}
