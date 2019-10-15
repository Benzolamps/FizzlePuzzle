using System.Diagnostics.CodeAnalysis;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PressurePlate : FizzleSwitch
    {
        private FizzlePuzzle.Item.PressurePlate __pressure_plate => __item as FizzlePuzzle.Item.PressurePlate;

        public PressurePlate(string name) : base(name, typeof(FizzlePuzzle.Item.PressurePlate))
        {
        }
        
        public string active_color => __pressure_plate.ActiveColor;

        public string deactive_color => __pressure_plate.DeactiveColor;

        protected override object __check_item => __item as FizzlePuzzle.Item.PressurePlate;
    }
}
