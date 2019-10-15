using FizzlePuzzle.Characters;
using FizzlePuzzle.Scene;

namespace fizzle_puzzle
{
    public class FirstFizzleCharacter : FizzleCharacter
    {
        internal FirstFizzleCharacter()
        {
        }

        internal override FizzleCharacterController __ctrl => FizzleScene.FirstPersonCtrl;

        internal override BaseCharacterAction __action => FizzleScene.FirstPersonCharAction;
    }
}
