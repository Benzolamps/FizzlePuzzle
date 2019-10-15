using System.Diagnostics.CodeAnalysis;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FizzleButton : FizzleSwitch
    {
        private FizzlePuzzle.Item.FizzleButton __button => __item as FizzlePuzzle.Item.FizzleButton;

        public FizzleButton(string name) : base(name, typeof(FizzlePuzzle.Item.FizzleButton))
        {
        }
        
        public string active_color => __button.ActiveColor;

        public string deactive_color => __button.DeactiveColor;

        protected override object __check_item => __item as FizzlePuzzle.Item.FizzleButton;
    }
}
