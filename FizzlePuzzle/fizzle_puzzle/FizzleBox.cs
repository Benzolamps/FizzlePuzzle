using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Core;
using FizzlePuzzle.Item;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FizzleBox : FizzleObject
    {
        private FizzlePuzzle.Item.FizzleBox __button => __item as FizzlePuzzle.Item.FizzleBox;

        public FizzleBox(string name) : base(name, typeof(FizzlePuzzle.Item.FizzleBox))
        {
            rewindable = __button.GetComponent<FizzleButtonRewindController>();
        }

        public bool carried => __button.Carrier;

        public bool rewindable { get; }

        public event FizzleEvent active
        {
            add
            {
                __button.carried += value.Invoke;
            }
            remove
            {
                __button.carried -= value.Invoke;
            }
        }

        public event FizzleEvent deactive
        {
            add
            {
                __button.released += value.Invoke;
            }
            remove
            {
                __button.released -= value.Invoke;
            }
        }

        protected override object __check_item => __button;
    }
}
