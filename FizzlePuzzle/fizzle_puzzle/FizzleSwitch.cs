using FizzlePuzzle.Item;
using System;
using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Core;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class FizzleSwitch : FizzleObject
    {
        private ISwitch __switch => __item as ISwitch;

        protected FizzleSwitch(string name, Type type) : base(name, type)
        {
        }

        public bool activated => __switch.Activated;

        public event FizzleEvent active
        {
            add
            {
                __switch.active += value.Invoke;
            }
            remove
            {
                __switch.active -= value.Invoke;
            }
        }

        public event FizzleEvent deactive
        {
            add
            {
                __switch.deactive += value.Invoke;
            }
            remove
            {
                __switch.deactive -= value.Invoke;
            }
        }
    }
}
