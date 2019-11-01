using FizzlePuzzle.TimeEffect;

namespace FizzlePuzzle.Item
{
    internal class FizzleLogicCurtainRewindController : RewindController
    {
        private FizzleLogicCurtain curtain;
        private bool lastActivate;

        protected override void Awake()
        {
            base.Awake();
            curtain = GetComponent<FizzleLogicCurtain>();
        }

        public override void BeginRewinding()
        {
            base.BeginRewinding();
            lastActivate = curtain.Activated;
        }

        protected override IRewindStatus PushToStack()
        {
            return new FizzleLogicCurtainRewindStatus
            {
                activate = curtain.Activated
            };
        }

        protected override void PopFromStack(IRewindStatus rewindStatus)
        {
            if (!(((FizzleLogicCurtainRewindStatus) rewindStatus).activate ^ lastActivate))
            {
                return;
            }
            curtain.ToggleActive();
            lastActivate = !lastActivate;
        }
    }
}
