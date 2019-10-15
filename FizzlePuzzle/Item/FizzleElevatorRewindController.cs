using FizzlePuzzle.Extension;
using FizzlePuzzle.TimeEffect;

namespace FizzlePuzzle.Item
{
    internal class FizzleElevatorRewindController : RewindController
    {
        private FizzleElevator elevator;

        protected override void Awake()
        {
            base.Awake();
            elevator = GetComponent<FizzleElevator>();
        }

        public override void BeginRewinding()
        {
            base.BeginRewinding();
            elevator.enabled = false;
        }

        public override void EndRewinding()
        {
            base.EndRewinding();
            elevator.enabled = true;
        }

        protected override IRewindStatus PushToStack()
        {
            return new FizzleElevatorRewindStatus
            {
                position = elevator.transform.position,
                status = elevator.status
            };
        }

        protected override void PopFromStack(IRewindStatus rewindStatus)
        {
            FizzleElevatorRewindStatus status = rewindStatus.Cast<FizzleElevatorRewindStatus>();
            elevator.transform.position = status.position;
            elevator.status = status.status;
        }
    }
}
