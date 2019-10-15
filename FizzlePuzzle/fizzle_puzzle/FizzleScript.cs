using System.Diagnostics.CodeAnalysis;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FizzleScript : FizzleObject
    {
        private FizzlePuzzle.Item.FizzlePythonScript __script => __item as FizzlePuzzle.Item.FizzlePythonScript;

        public FizzleScript(string name) : base(name, typeof(FizzlePuzzle.Item.FizzlePythonScript))
        {
        }

        public string path => __script.m_Path;

        public string code => __script.Code;

        public void execute()
        {
            __script.Execute();
        }

        protected override object __check_item => __script;
    }
}
