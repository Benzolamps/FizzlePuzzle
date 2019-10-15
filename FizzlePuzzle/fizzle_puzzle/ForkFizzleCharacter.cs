using FizzlePuzzle.Characters;
using FizzlePuzzle.Scene;

namespace fizzle_puzzle
{
    public class ForkFizzleCharacter : FizzleCharacter
    {
        internal ForkFizzleCharacter()
        {
        }

        internal override FizzleCharacterController __ctrl => FizzleScene.ForkCtrl;

        internal override BaseCharacterAction __action => FizzleScene.ForkCharAction;
    }
}
