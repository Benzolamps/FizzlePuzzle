using System.Diagnostics.CodeAnalysis;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FizzleTrigger : FizzleSwitch
    {
        private FizzlePuzzle.Item.FizzleTrigger __trigger => __item as FizzlePuzzle.Item.FizzleTrigger;

        public FizzleTrigger(string name) : base(name, typeof(FizzlePuzzle.Item.FizzleTrigger))
        {
        }

        public string trigger_type => __trigger._TriggerType.ToString();

        public void recharge()
        {
            __trigger.Recharge();
        }

        protected override object __check_item => __trigger;
    }
}
