using System;
using System.Diagnostics.CodeAnalysis;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class FizzleResponse : FizzleObject
    {
        public abstract string activator { get; }

        protected FizzleResponse(string name, Type type) : base(name, type)
        {
        }
    }
}
