using System.Linq;
using UnityEngine;

namespace FizzlePuzzle.Extension
{
    internal struct FizzleLayerMask
    {
        internal static readonly FizzleLayerMask EVERYTHING = new FizzleLayerMask
        {
            value = uint.MaxValue
        };

        internal static readonly FizzleLayerMask NOTHING = new FizzleLayerMask
        {
            value = 0
        };

        internal uint value;

        internal static FizzleLayerMask GetMask(params string[] args)
        {
            return args.Aggregate(NOTHING, (current, layerName) => (FizzleLayerMask) (current | 1 << LayerMask.NameToLayer(layerName)));
        }

        internal static FizzleLayerMask GetNotMask(params string[] args)
        {
            return EVERYTHING & ~GetMask(args);
        }

        public static implicit operator int(FizzleLayerMask mask)
        {
            return (int) mask.value;
        }

        public static implicit operator FizzleLayerMask(int value)
        {
            return new FizzleLayerMask {value = (uint) value};
        }
    }
}