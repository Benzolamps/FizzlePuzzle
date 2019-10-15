using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FizzleBarrier : FizzleResponse
    {
        private FizzlePuzzle.Item.FizzleBarrier __barrier => __item as FizzlePuzzle.Item.FizzleBarrier;

        public FizzleBarrier(string name) : base(name, typeof(FizzlePuzzle.Item.FizzleBarrier))
        {
        }

        public override string activator => __barrier.Activator;

        public bool opening => __barrier.Opening;

        public string color => __barrier.Color;

        public event FizzleEvent opened
        {
            add
            {
                __barrier.opened += value.Invoke;
            }
            remove
            {
                __barrier.opened -= value.Invoke;
            }
        }

        public event FizzleEvent closed
        {
            add
            {
                __barrier.closed += value.Invoke;
            }
            remove
            {
                __barrier.closed -= value.Invoke;
            }
        }

        public void open()
        {
            __barrier.Open();
        }

        public void close()
        {
            __barrier.Close();
        }

        protected override object __check_item => __barrier;
    }
}
