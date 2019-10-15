using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Core;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FizzleElevator : FizzleResponse
    {
        private FizzlePuzzle.Item.FizzleElevator __elevator => __item as FizzlePuzzle.Item.FizzleElevator;

        public FizzleElevator(string name) : base(name, typeof(FizzlePuzzle.Item.FizzleElevator))
        {
        }

        public override string activator => __elevator.Activator;

        public float height => __elevator.Height;

        public string status => __elevator.status.ToString();

        public event FizzleEvent raised
        {
            add
            {
                __elevator.raised += value.Invoke;
            }
            remove
            {
                __elevator.raised -= value.Invoke;
            }
        }

        public event FizzleEvent dropped
        {
            add
            {
                __elevator.dropped += value.Invoke;
            }
            remove
            {
                __elevator.dropped -= value.Invoke;
            }
        }

        public event FizzleEvent raise_finished
        {
            add
            {
                __elevator.raiseFinished += value.Invoke;
            }
            remove
            {
                __elevator.raiseFinished -= value.Invoke;
            }
        }

        public event FizzleEvent drop_finished
        {
            add
            {
                __elevator.dropFinished += value.Invoke;
            }
            remove
            {
                __elevator.dropFinished -= value.Invoke;
            }
        }

        public void raise_()
        {
            __elevator.Raise();
        }

        public void drop()
        {
            __elevator.Drop();
        }

        protected override object __check_item => __elevator;
    }
}
