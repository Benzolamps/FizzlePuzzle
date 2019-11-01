using System.Linq;
using UnityEngine;

namespace FizzlePuzzle.Extension
{
    internal struct FizzleLayerMask
    {
        internal static readonly FizzleLayerMask everything = new FizzleLayerMask
        {
            value = uint.MaxValue
        };

        internal static readonly FizzleLayerMask nothing = new FizzleLayerMask
        {
            value = 0
        };

        internal uint value;

        internal static FizzleLayerMask GetMask(params string[] args)
        {
            return args.Aggregate(nothing, (current, layerName) => (FizzleLayerMask) (current | 1 << LayerMask.NameToLayer(layerName)));
        }

        internal static FizzleLayerMask GetNotMask(params string[] args)
        {
            return everything & ~GetMask(args);
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
