using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Extension;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FizzleLogicCurtain : FizzleSwitch
    {
        private FizzlePuzzle.Item.FizzleLogicCurtain __logicCurtain => __item as FizzlePuzzle.Item.FizzleLogicCurtain;

        public FizzleLogicCurtain(string name) : base(name, typeof(FizzlePuzzle.Item.FizzleLogicCurtain))
        {
        }

        public string active_color => __logicCurtain.ActiveColor;

        public string deactive_color => __logicCurtain.DeactiveColor;

        protected override object __check_item => __logicCurtain;
    }
}
