using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Characters;
using FizzlePuzzle.Core;
using FizzlePuzzle.Scene;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class FizzleCharacter
    {
        internal abstract FizzleCharacterController __ctrl { get; }

        internal abstract BaseCharacterAction __action { get; }

        internal FizzleCharacter()
        {
        }

        public FizzleBox carrying_object => !__action.carryingObject ? null : new FizzleBox(ItemWrapper.GetWrapper(__action.carryingObject).name);

        public float distance => __action.Distance;

        public event FizzleEvent<FizzleBox> carried_object
        {
            add
            {
                __action.carriedObject += box => value(FizzleObject.__convert<FizzleBox>(box));
            }
            remove
            {
                throw new System.NotImplementedException();
            }
        }

        public event FizzleEvent<FizzleBox> released_object
        {
            add
            {
                __action.releasedObject += box => value(FizzleObject.__convert<FizzleBox>(box));
            }
            remove
            {
                throw new System.NotImplementedException();
            }
        }

        public event FizzleEvent<FizzleButton> pressed_button
        {
            add
            {
                __action.pressedButton += btn => value(FizzleObject.__convert<FizzleButton>(btn));
            }
            remove
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
